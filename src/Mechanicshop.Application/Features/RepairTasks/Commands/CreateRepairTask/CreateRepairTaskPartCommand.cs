using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record CreateRepairTaskPartCommand(
    string Name,
    decimal Cost,
    int Quantity) : IRequest<Result<Success>>;   