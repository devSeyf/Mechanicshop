using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.AssignLabor;

public sealed record AssignLaborCommand(Guid WorkOrderId, Guid LaborId) : IRequest<Result<Updated>>;