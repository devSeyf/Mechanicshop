namespace Mechanicshop.Application.Features.Scheduling.DTOs;

public class ScheduleDTO
{
    public DateOnly OnDate { get; set; }
    public bool EndOfDay { get; set; }
    public List<SpotDTO> Spots { get; set; } = [];
}