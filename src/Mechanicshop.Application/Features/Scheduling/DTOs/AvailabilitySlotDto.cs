using Mechanicshop.Application.Features.Labors.DTOs;
using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Domain.WorkOrder.Enums;

namespace Mechanicshop.Application.Features.Scheduling.DTOs;

public class AvailabilitySlotDTO
{
    public Guid? WorkOrderId { get; set; }
    public Spot Spot { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public string? Vehicle { get; set; }
    public LaborDTO? Labor { get; set; }
    public bool IsOccupied { get; set; }
    public bool? IsAvailable { get; set; }
    public bool WorkOrderLocked { get; set; }
    public WorkOrderState? State { get; set; }
    public RepairTaskDTO[]? RepairTasks { get; set; }
}