using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Features.RepairTasks.Queries.GetRepairTaskById;

public sealed record GetRepairTaskByIdQuery(Guid RepairTaskId) : ICachedQuery<Result<RepairTaskDTO>>
{
    public string CacheKey => $"repair-task_{RepairTaskId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["repair-task"];
}