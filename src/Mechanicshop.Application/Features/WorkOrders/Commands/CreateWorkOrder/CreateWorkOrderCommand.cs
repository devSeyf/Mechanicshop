using Mechanicshop.Application.Features.WorkOrders.DTOs;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder.Enums;
using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public sealed record CreateWorkOrderCommand(
    Spot Spot,
    Guid VehicleId,
    DateTimeOffset StartAt,
    List<Guid> RepairTaskIds,
    Guid? LaborId)
: IRequest<Result<WorkOrderDTO>>;