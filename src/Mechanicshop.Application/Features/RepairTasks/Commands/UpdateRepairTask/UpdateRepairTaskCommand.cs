using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.ReapairTasks.Enums;
using MediatR;

namespace Mechanicshop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public sealed record UpdateRepairTaskCommand(
    Guid RepairTaskId,
    string Name,
    decimal LaborCost,
    RepairDurationInMinutes EstimatedDurationInMins,
    List<UpdateRepairTaskPartCommand> Parts
) : IRequest<Result<Updated>>;