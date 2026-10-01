using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.RelocateWorkOrder;

public class RelocateWorkOrderCommandHandler(
    ILogger<RelocateWorkOrderCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache,
    IWorkOrderPolicy appointmentValidator
    ) : IRequestHandler<RelocateWorkOrderCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(RelocateWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await context.WorkOrders
        .Include(a => a.RepairTasks)
        .Include(a => a.Labor)
        .Include(a => a.Vehicle)
        .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, cancellationToken);

        if (workOrder is null)
        {
            logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        var duration = workOrder.EndAtUtc.Subtract(workOrder.StartAtUtc).Duration();

        var endAt = command.NewStartAt.Add(duration);

        var checkSpotAvailabilityResult = await appointmentValidator.CheckSpotAvailabilityAsync(
            workOrder.Spot,
            command.NewStartAt,
            endAt,
            excludeWorkOrderId: workOrder.Id,
            cancellationToken);

        if (checkSpotAvailabilityResult.IsError)
        {
            logger.LogError("Spot: {Spot} is not available.", workOrder.Spot.ToString());

            return checkSpotAvailabilityResult.Errors;
        }

        if (await appointmentValidator.IsLaborOccupied(workOrder.LaborId, command.WorkOrderId, command.NewStartAt, endAt))
        {
            logger.LogError("Labor with Id '{LaborId}' is already occupied during the requested time.", workOrder.LaborId);

            return ApplicationErrors.LaborOccupied;
        }

        if (await appointmentValidator.IsVehicleAlreadyScheduled(workOrder.VehicleId, command.NewStartAt, endAt, command.WorkOrderId))
        {
            logger.LogError("Vehicle with Id '{VehicleId}' already has an overlapping WorkOrder.", workOrder.VehicleId);

            return ApplicationErrors.VehicleSchedulingConflict;
        }

        var updateTimingResult = workOrder.UpdateTiming(command.NewStartAt, endAt);

        if (updateTimingResult.IsError)
        {
            logger.LogError("Failed to update timing: {Error}", updateTimingResult.TopError.Description);

            return updateTimingResult.Errors;
        }

        var updateSpotResult = workOrder.UpdateSpot(command.NewSpot);

        if (updateTimingResult.IsError)
        {
            logger.LogError("Failed to update Spot: {Error}", updateSpotResult.TopError.Description);

            return updateTimingResult.Errors;
        }

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await context.SaveChangesAsync(cancellationToken);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await cache.RemoveByTagAsync("work-order", cancellationToken);

        return Result.Updated;

    }
}