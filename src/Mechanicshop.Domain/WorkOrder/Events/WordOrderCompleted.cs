using Mechanicshop.Domain.Common;

namespace Mechanicshop.Domain.WorkOrder.Events;

public sealed class WorkOrderCompleted : DomainEvent
{
    public Guid WorkOrderId { get; set; }
}