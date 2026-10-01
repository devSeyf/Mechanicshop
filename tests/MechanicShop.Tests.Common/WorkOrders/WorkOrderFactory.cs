using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.ReapairTasks;
using Mechanicshop.Domain.WorkOrder;
using Mechanicshop.Domain.WorkOrder.Enums;
using MechanicShop.Tests.Common.RepaireTasks;

namespace MechanicShop.Tests.Common.WorkOrders;

public static class WorkOrderFactory
{
    public static Result<WorkOrder> CreateWorkOrder(
        Guid? id = null,
        Guid? vehicleId = null,
        DateTimeOffset? startAt = null,
        DateTimeOffset? endAt = null,
        Guid? laborId = null,
        Spot? spot = null,
        List<RepairTask>? repairTasks = null)
    {
        return WorkOrder.Create(
            id ?? Guid.NewGuid(),
            vehicleId ?? Guid.NewGuid(),
            startAt ?? DateTimeOffset.UtcNow,
            endAt ?? DateTimeOffset.UtcNow.AddHours(1),
            laborId ?? Guid.NewGuid(),
            spot ?? Spot.A,
            repairTasks ?? [RepairTaskFactory.CreateRepairTask().Value]);
    }
}