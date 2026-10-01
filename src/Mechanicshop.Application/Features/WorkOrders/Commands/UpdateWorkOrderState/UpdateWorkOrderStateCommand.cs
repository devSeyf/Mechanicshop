using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder.Enums;
using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.UpdateWorkOrderState;

public sealed record UpdateWorkOrderStateCommand(
    Guid WorkOrderId,
    WorkOrderState State) : IRequest<Result<Updated>>;