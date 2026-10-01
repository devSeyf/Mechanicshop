using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder.Enums;
using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.RelocateWorkOrder;


public sealed record RelocateWorkOrderCommand(
    Guid WorkOrderId,
    DateTimeOffset NewStartAt,
    Spot NewSpot) : IRequest<Result<Updated>>;