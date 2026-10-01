using Mechanicshop.Application.Features.Customers.Mappers;
using Mechanicshop.Application.Features.Labors.DTOs;
using Mechanicshop.Application.Features.RepairTasks.Mappers;
using Mechanicshop.Application.Features.WorkOrders.DTOs;
using Mechanicshop.Domain.WorkOrder;

namespace Mechanicshop.Application.Features.WorkOrders.Mappers;

public static class WorkOrderMapper
{
    public static WorkOrderDTO ToDto(this WorkOrder entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new WorkOrderDTO
        {
            WorkOrderId = entity.Id,
            Spot = entity.Spot,
            StartAtUtc = entity.StartAtUtc,
            EndAtUtc = entity.EndAtUtc,
            Labor = entity.Labor is null ? null : new LaborDTO
            {
                LaborId = entity.LaborId,
                Name = $"{entity.Labor.FirstName} {entity.Labor.LastName}"
            },
            RepairTasks = entity.RepairTasks.ToDtos(),
            Vehicle = entity.Vehicle is null ? null : entity.Vehicle.ToDTO(),
            State = entity.State,
            TotalPartCost = entity.RepairTasks.SelectMany(t => t.Parts).Sum(p => p.Cost * p.Quantity),
            TotalLaborCost = entity.RepairTasks.Sum(p => p.LaborCost),
            TotalCost = entity.RepairTasks.Sum(rt => rt.TotalCost),
            TotalDurationInMins = entity.RepairTasks.Sum(rt => (int)rt.EstimatedDurationInMins),
            InvoiceId = entity.Invoice?.Id,
            CreatedAt = entity.CreatedAtUTC
        };
    }

    public static List<WorkOrderDTO> ToDtos(this IEnumerable<WorkOrder> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }

    public static WorkOrderListItemDTO ToListItemDTO(this WorkOrder entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new WorkOrderListItemDTO
        {
            WorkOrderId = entity.Id,
            Spot = entity.Spot,
            StartAtUtc = entity.StartAtUtc,
            EndAtUtc = entity.EndAtUtc,
            Vehicle = entity.Vehicle!.ToDTO(),
            Labor = entity.Labor is null ? null :
                $"{entity.Labor.FirstName} {entity.Labor.LastName}",
            State = entity.State,
            RepairTasks = entity.RepairTasks.Select(rt => rt.Name).ToList()
        };
    }
}