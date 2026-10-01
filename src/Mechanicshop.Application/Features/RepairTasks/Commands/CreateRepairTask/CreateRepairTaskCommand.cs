using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.ReapairTasks.Enums;
using MediatR;

namespace Mechanicshop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record CreateRepairTaskCommand (
    string? Name,
    decimal LaborCost,
    RepairDurationInMinutes? EstimatedDurationInMins,
    List<CreateRepairTaskPartCommand> Parts) : IRequest<Result<RepairTaskDTO>>;
