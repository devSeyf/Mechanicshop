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

namespace Mechanicshop.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public class DeleteWorkOrderCommandHandler(
    ILogger<DeleteWorkOrderCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<DeleteWorkOrderCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteWorkOrderCommand command, CancellationToken ct)
    {
        var workOrder = await context.WorkOrders
            .FirstOrDefaultAsync(a => a.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        if (workOrder.State is not WorkOrderState.Scheduled)
        {
            logger.LogError(
                "Deletion failed: only 'Scheduled' or 'Confirmed' WorkOrders can be deleted. Current status: {Status}",
                workOrder.State);

            return WorkOrderErrors.Readonly;
        }

        context.WorkOrders.Remove(workOrder);

        await context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await cache.RemoveByTagAsync("work-order", ct);

        return Result.Deleted;
    }
}