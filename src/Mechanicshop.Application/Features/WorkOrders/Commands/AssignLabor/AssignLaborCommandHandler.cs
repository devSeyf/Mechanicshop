using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.AssignLabor;


public class AssignLaborCommandHandler(
    ILogger<AssignLaborCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache,
    IWorkOrderPolicy workOrderValidator) : IRequestHandler<AssignLaborCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(AssignLaborCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await context.WorkOrders
        .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, cancellationToken);

        if (workOrder is null)
        {
            logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);
            return ApplicationErrors.WorkOrderNotFound;
        }

        var labor = await context.Employees.FindAsync([command.LaborId], cancellationToken);

        if (labor is null)
        {
            logger.LogError("Invalid LaborId: {LaborId}", command.LaborId);
            return ApplicationErrors.LaborNotFound;
        }

        if (await workOrderValidator.IsLaborOccupied(command.LaborId, command.WorkOrderId, workOrder.StartAtUtc, workOrder.EndAtUtc))
        {
            logger.LogError("Labor with Id '{LaborId}' is already occupied during the requested time.", workOrder.LaborId);
            return ApplicationErrors.LaborOccupied;
        }

        var updateLaborResult = workOrder.UpdateLabor(command.LaborId);

        if (updateLaborResult.IsError)
        {
            foreach (var error in updateLaborResult.Errors)
            {
                logger.LogError("[LaborUpdate] {ErrorCode}: {ErrorDescription}", error.Code, error.Description);
            }
            return updateLaborResult.Errors;
        }

        await context.SaveChangesAsync(cancellationToken);

        await cache.RemoveByTagAsync("work-order", cancellationToken);

        return Result.Updated;
    }
}