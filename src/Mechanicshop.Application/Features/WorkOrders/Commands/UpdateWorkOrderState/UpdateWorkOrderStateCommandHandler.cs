using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder;
using Mechanicshop.Domain.WorkOrder.Enums;
using Mechanicshop.Domain.WorkOrder.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.UpdateWorkOrderState;

public class UpdateWorkOrderStateCommandHandler(
    ILogger<UpdateWorkOrderStateCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache,
    TimeProvider dateTime
    )
    : IRequestHandler<UpdateWorkOrderStateCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateWorkOrderStateCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await context.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, cancellationToken);

        if (workOrder is null)
        {
            logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        if (workOrder.StartAtUtc > dateTime.GetUtcNow())
        {
            logger.LogError("State transition for WorkOrder Id '{WorkOrderId}` is not allowed before the work order�s scheduled start time.", command.WorkOrderId);

            return WorkOrderErrors.StateTransitionNotAllowed(workOrder.StartAtUtc);
        }

        var updateStatusResult = workOrder.UpdateState(command.State);

        if (updateStatusResult.IsError)
        {
            logger.LogError("Failed to update status: {Error}", updateStatusResult.TopError.Description);

            return updateStatusResult.Errors;
        }

        if (command.State == WorkOrderState.Completed)
        {
            workOrder.AddDomainEvent(new WorkOrderCompleted { WorkOrderId = command.WorkOrderId });
        }

        await context.SaveChangesAsync(cancellationToken);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await cache.RemoveByTagAsync("work-order", cancellationToken);

        return Result.Updated;
    }
}