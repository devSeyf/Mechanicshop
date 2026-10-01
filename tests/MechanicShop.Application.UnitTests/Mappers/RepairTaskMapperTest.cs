using Mechanicshop.Application.Features.RepairTasks.Mappers;
using Mechanicshop.Domain.ReapairTasks;
using Mechanicshop.Domain.ReapairTasks.Enums;
using Mechanicshop.Domain.ReapairTasks.Parts;
using MechanicShop.Tests.Common.RepaireTasks;
using Xunit;

namespace MechanicShop.Application.UnitTests.Mappers;

public class RepairTaskMapperTest
{
    [Fact]
    public void ToDto_ShouldMapCorrectly()
    {
        var part = PartFactory.CreatePart(
            Guid.NewGuid(),
            "Oil Filter",
            50m,
            1).Value;

        var repairTask = RepairTaskFactory.CreateRepairTask(
            Guid.NewGuid(),
            "Oil Change",
            100m,
            RepairDurationInMinutes.Min30,
            [part]).Value;

        
        // Act
        var dto = repairTask.ToDto();

        // Assert
        Assert.Equal(repairTask.Id, dto.RepairTaskId);
        Assert.Equal(repairTask.Name, dto.Name);
        Assert.Equal(repairTask.LaborCost, dto.LaborCost);
        Assert.Equal(repairTask.TotalCost, dto.TotalCost);
        Assert.Equal(repairTask.EstimatedDurationInMins, dto.EstimatedDurationInMins);
        //Assert.Equal(repairTask.Parts.ToDtos(), dto.Parts);

        Assert.Single(dto.Parts);
        Assert.Equal(part.Id, dto.Parts[0].PartId);
        Assert.Equal(part.Name, dto.Parts[0].Name);
    }

    [Fact]
    public void RepairTaskToDtos_ShouldMapListCorrectly()
    {
        // Arrange
        var part = PartFactory.CreatePart(Guid.NewGuid(), "Brake Pad", 150m, 2).Value;
        var repairTask = RepairTaskFactory.CreateRepairTask(
            Guid.NewGuid(),
            "Brake Replacement",
            200m,
            RepairDurationInMinutes.Min60,
            [part]
        ).Value;

        var repairTasks = new List<RepairTask> { repairTask };

        // Act
        var dtos = repairTasks.ToDtos();

        // Assert
        Assert.Single(dtos);
        Assert.Equal(repairTasks[0].Id, dtos[0].RepairTaskId);
        Assert.Equal(repairTasks[0].Name, dtos[0].Name);
    }

    [Fact]
    public void PartToDto_ShouldMapCorrectly()
    {
        // Arrange
        var part = PartFactory.CreatePart(
            id: Guid.NewGuid(),
            name: "Spark Plug",
            cost: 20m,
            quantity: 4
        ).Value;

        // Act
        var dto = part.ToDto();

        // Assert
        Assert.Equal(part.Id, dto.PartId);
        Assert.Equal(part.Name, dto.Name);
        Assert.Equal(part.Cost, dto.Cost);
        Assert.Equal(part.Quantity, dto.Quantity);
    }

    [Fact]
    public void PartToDtos_ShouldMapListCorrectly()
    {
        // Arrange
        var part1 = PartFactory.CreatePart(Guid.NewGuid(), "Air Filter", 30m, 1).Value;
        var part2 = PartFactory.CreatePart(Guid.NewGuid(), "Cabin Filter", 25m, 1).Value;

        var parts = new List<Part> { part1, part2 };

        // Act
        var dtos = parts.ToDtos();

        // Assert
        Assert.Equal(2, dtos.Count);
        Assert.Equal(parts[0].Id, dtos[0].PartId);
        Assert.Equal(parts[1].Id, dtos[1].PartId);
    }

    // --- Edge Cases (Null Checks) ---

    [Fact]
    public void RepairTaskToDto_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Arrange
        RepairTask? repairTask = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => repairTask!.ToDto());
    }

    [Fact]
    public void PartToDto_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Arrange
        Part? part = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => part!.ToDto());
    }
}