using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.ReapairTasks;
using Mechanicshop.Domain.ReapairTasks.Enums;
using Mechanicshop.Domain.ReapairTasks.Parts;

namespace MechanicShop.Tests.Common.RepaireTasks;

public static class RepairTaskFactory
{
    public static Result<RepairTask> CreateRepairTask(
        Guid? id = null,
        string? name = null,
        decimal? laborCost = null,
        RepairDurationInMinutes? repairDurationInMinutes = null,
        List<Part>? parts = null)
    {
        return RepairTask.Create(
            id ?? Guid.NewGuid(),
            name ?? "Brake Inspection",
            laborCost ?? 100,
            repairDurationInMinutes ?? RepairDurationInMinutes.Min30,
            parts ?? [Part.Create(Guid.NewGuid(), "Brake Pads", 50, 1).Value]);
    }
}