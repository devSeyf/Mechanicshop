using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Features.RepairTasks.Queries.GetRepairTasks;

public sealed record GetRepairTasksQuery() : ICachedQuery<Result<List<RepairTaskDTO>>>
{
    public string CacheKey => "repair-tasks";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["repair-tasks"];
}