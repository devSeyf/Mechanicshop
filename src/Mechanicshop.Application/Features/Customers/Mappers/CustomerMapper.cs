using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.Customers;
using Mechanicshop.Domain.Customers.Vehicles;

namespace Mechanicshop.Application.Features.Customers.Mappers;

public static class CustomerMapper
{
    public static CustomerDTO ToDTO(this Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerDTO
        {
            CustomerId = entity.Id,
            Name = entity.Name!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!,
            Vehicles = entity.Vehicles?.Select(v => v.ToDTO()).ToList() ?? []
        };
    }

    public static List<CustomerDTO> ToDTOs(this IEnumerable<Customer> entities)
    {
        return [.. entities.Select(e => e.ToDTO())];
    }

    public static VehicleDTO ToDTO(this Vehicle entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new VehicleDTO(entity.Id, entity.Make!, entity.Model!, entity.Year, entity.LicensePlate!);
    }

    public static List<VehicleDTO> ToDTOs(this IEnumerable<Vehicle> entities)
    {
        return [.. entities.Select(e => e.ToDTO())];
    }
}