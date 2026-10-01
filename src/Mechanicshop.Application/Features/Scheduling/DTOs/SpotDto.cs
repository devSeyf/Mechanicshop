using Mechanicshop.Application.Features.Scheduling.DTOs;
using Mechanicshop.Domain.WorkOrder.Enums;

namespace Mechanicshop.Application.Features.Scheduling.DTOs;

public class SpotDTO
{
    public Spot Spot { get; set; }
    public List<AvailabilitySlotDTO> Slots { get; set; } = [];
}