namespace Mechanicshop.Application.Features.Customers.DTOs;


public class CustomerDTO
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<VehicleDTO> Vehicles { get; set; } = [];
}