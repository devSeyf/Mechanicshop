using Mechanicshop.Application.Features.Labors.Mappers;
using Mechanicshop.Domain.Employees;
using MechanicShop.Tests.Common.Employees;
using Xunit;

namespace MechanicShop.Application.UnitTests.Mappers;

public class LaborMapperTest
{
    [Fact]
    public void ToDto_ShouldMapCorrectly()
    {
        var labor = EmployeeFactory.CreateLabor(
            Guid.NewGuid(),
            "Ahmed",
            "Ali").Value;

        var dto = labor.ToDto();

        Assert.Equal(labor.Id, dto.LaborId);
        Assert.Equal(labor.FullName, dto.Name);
    }

    [Fact]
    public void ToDtos_ShouldMapLaborListCorrectly()
    {
        var labor1 = EmployeeFactory.CreateLabor(Guid.NewGuid(), "Ahmed", "Ali").Value;
        var labor2 = EmployeeFactory.CreateLabor(Guid.NewGuid(), "Mohamed", "Omar").Value;

        var labors = new List<Employee> { labor1, labor2 };

        var dtos = labors.ToDtos();

        Assert.Equal(2, dtos.Count);
        Assert.Equal(labors[0].Id, dtos[0].LaborId);
        Assert.Equal(labors[0].FullName, dtos[0].Name);

        Assert.Equal(labors[1].Id, dtos[1].LaborId);
        Assert.Equal(labors[1].FullName, dtos[1].Name);
    }

    [Fact]
    public void ToDto_ShouldThrowNullReferenceException_WhenEmployeeIsNull()
    {
        Employee? labor = null;

        Assert.Throws<NullReferenceException>(() => labor!.ToDto());
    }
}