using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Domain.WorkOrder.Events;

using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.EventHandlers;

public sealed class WorkOrderCollectionModifiedEventHandler(IWorkOrderNotifier notifier)
        : INotificationHandler<WorkOrderCollectionModified>
{
    private readonly IWorkOrderNotifier _notifier = notifier;

    public Task Handle(WorkOrderCollectionModified notification, CancellationToken ct) =>
        _notifier.NotifyWorkOrdersChangedAsync(ct);
}