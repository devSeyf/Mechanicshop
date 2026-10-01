namespace Mechanicshop.Application.Features.RepairTasks.DTOs;

public class PartDTO
{
    public Guid PartId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int Quantity { get; set; }
}