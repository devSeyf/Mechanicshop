namespace Mechanicshop.Application.Features.Customers.DTOs;

public sealed record VehicleDTO(Guid VehicleId, string Make, string Model, int Year, string LicensePlate);