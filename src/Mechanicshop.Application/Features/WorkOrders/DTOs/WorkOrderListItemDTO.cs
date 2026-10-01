using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.WorkOrder.Enums;

namespace Mechanicshop.Application.Features.WorkOrders.DTOs;

public class WorkOrderListItemDTO
{
    public Guid WorkOrderId { get; set; }
    public Guid? InvoiceId { get; set; }
    public VehicleDTO Vehicle { get; set; } = default!;
    public string? Customer { get; set; }
    public string? Labor { get; set; }
    public WorkOrderState State { get; set; }
    public Spot Spot { get; set; }
    public DateTimeOffset StartAtUtc { get; set; }
    public DateTimeOffset EndAtUtc { get; set; }
    public List<string> RepairTasks { get; set; } = [];
}