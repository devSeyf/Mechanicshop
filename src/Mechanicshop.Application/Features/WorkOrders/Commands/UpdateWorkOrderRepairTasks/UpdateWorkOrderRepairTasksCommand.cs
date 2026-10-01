using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.WorkOrders.Commands.UpdateWorkOrderRepairTasks;

public sealed record UpdateWorkOrderRepairTasksCommand(
    Guid WorkOrderId,
    Guid[] RepairTaskIds) : IRequest<Result<Updated>>;