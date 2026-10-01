using Mechanicshop.Application.Features.Labors.DTOs;
using Mechanicshop.Domain.Employees;

namespace Mechanicshop.Application.Features.Labors.Mappers;

public static class LaborMapper
{
    public static LaborDTO ToDto(this Employee employee)
    {
        return new LaborDTO { LaborId = employee.Id, Name = employee.FullName };
    }

    public static List<LaborDTO> ToDtos(this IEnumerable<Employee> entities)
    {
        return [.. entities.Select(l => l.ToDto())];
    }
}