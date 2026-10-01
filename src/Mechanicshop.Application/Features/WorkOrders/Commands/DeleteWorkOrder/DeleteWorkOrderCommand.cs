using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public sealed record DeleteWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<Deleted>>;