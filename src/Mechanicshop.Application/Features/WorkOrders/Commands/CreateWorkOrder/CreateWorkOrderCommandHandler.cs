using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.WorkOrders.DTOs;
using Mechanicshop.Application.Features.WorkOrders.Mappers;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder;
using Mechanicshop.Domain.WorkOrder.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderCommandHandler(
    IAppDbContext context,
    ILogger<CreateWorkOrderCommandHandler> logger,
    HybridCache cache,
    IWorkOrderPolicy workOrderValidator
    ) : IRequestHandler<CreateWorkOrderCommand, Result<WorkOrderDTO>>
{
    public async Task<Result<WorkOrderDTO>> Handle(CreateWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var repairTasks = await context.RepairTasks
        .Where(t => command.RepairTaskIds.Contains(t.Id))
        .ToListAsync(cancellationToken);

        if (repairTasks.Count != command.RepairTaskIds.Count)
        {
            var missingIds = command.RepairTaskIds.Except(repairTasks.Select(t => t.Id)).ToArray();

            logger.LogError("Some RepairTaskIds not found: {MissingIds}", string.Join(", ", missingIds));

            return ApplicationErrors.RepairTaskNotFound;
        }

        var totalEstimatedDuration = TimeSpan.FromMinutes(repairTasks.Sum(r => (int)r.EstimatedDurationInMins));
        var endAt = command.StartAt.Add(totalEstimatedDuration);

        if (workOrderValidator.IsOutsideOperatingHours(command.StartAt, totalEstimatedDuration))
        {
            logger.LogError("The WorkOrder time ({StartAt} ? {EndAt}) is outside of store operating hours.", command.StartAt, endAt);

            return ApplicationErrors.WorkOrderOutsideOperatingHour(command.StartAt, endAt);
        }

        var checkMinRequirementResult = workOrderValidator.ValidateMinimumRequirement(command.StartAt, endAt);

        if (checkMinRequirementResult.IsError)
        {
            logger.LogError("WorkOrder duration is shorter than the configured minimum.");

            return checkMinRequirementResult.Errors;
        }

        var checkSpotAvailabilityResult = await workOrderValidator.CheckSpotAvailabilityAsync(
            command.Spot,
            command.StartAt,
            endAt,
            excludeWorkOrderId: null,
            cancellationToken);

        if (checkSpotAvailabilityResult.IsError)
        {
            logger.LogError("Spot: {Spot} is not available.", command.Spot.ToString());
            return checkSpotAvailabilityResult.Errors;
        }

        var vehicle = await context.Vehicles.Include(v => v.Customer).FirstOrDefaultAsync(v => v.Id == command.VehicleId, cancellationToken: cancellationToken);

        if (vehicle is null)
        {
            logger.LogError("Vehicle with Id '{VehicleId}' does not exist.", command.VehicleId);

            return ApplicationErrors.VehicleNotFound;
        }

        var labor = await context.Employees.FindAsync([command.LaborId], cancellationToken);

        if (labor is null)
        {
            logger.LogError("Invalid LaborId: {LaborId}", command.LaborId.ToString());
            return ApplicationErrors.LaborNotFound;
        }

        var hasVehicleConflict = await context.WorkOrders
            .AnyAsync(
                a =>
                a.VehicleId == command.VehicleId &&
                a.StartAtUtc.Date == command.StartAt.Date &&
                a.StartAtUtc < endAt &&
                a.EndAtUtc > command.StartAt,
                cancellationToken);

        if (hasVehicleConflict)
        {
            logger.LogError("Vehicle with Id '{VehicleId}' already has an overlapping WorkOrder.", command.VehicleId);
            return Error.Conflict(
                code: "VehicleOverlappingWorkOrders",
                description: "The vehicle already has an overlapping WorkOrder.");
        }

        var isLaborOccupied = await context.WorkOrders
            .AnyAsync(
                a =>
                a.LaborId == command.LaborId &&
                a.StartAtUtc < endAt &&
                a.EndAtUtc > command.StartAt,
                cancellationToken);

        if (isLaborOccupied)
        {
            logger.LogError("Labor with Id '{LaborId}' is already occupied during the requested time.", command.LaborId);
            return Error.Conflict(
                code: "LaborOccupied",
                description: "Labor is already occupied during the requested time.");
        }

        var createWorkOrderResult = WorkOrder.Create(
            Guid.NewGuid(),
            command.VehicleId,
            command.StartAt,
            endAt,
            command.LaborId!.Value,
            command.Spot,
            repairTasks);

        if (createWorkOrderResult.IsError)
        {
            logger.LogError("Failed to create WorkOrder: {Error}", createWorkOrderResult.TopError.Description);

            return createWorkOrderResult.Errors;
        }

        var workOrder = createWorkOrderResult.Value;

        context.WorkOrders.Add(workOrder);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await context.SaveChangesAsync(cancellationToken);

        workOrder.Vehicle = vehicle;
        workOrder.Labor = labor;

        logger.LogInformation("WorkOrder with Id '{WorkOrderId}' created successfully.", workOrder.Id);

        await cache.RemoveByTagAsync("work-order", cancellationToken);

        return workOrder.ToDto();
    }
}