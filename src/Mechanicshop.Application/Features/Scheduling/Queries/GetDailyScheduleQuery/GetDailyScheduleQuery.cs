using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Scheduling.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Features.Scheduling.Queries.GetDailyScheduleQuery;

public sealed record GetDailyScheduleQuery(
    TimeZoneInfo TimeZone,
    DateOnly ScheduleDate,
    Guid? LaborId = null) : ICachedQuery<Result<ScheduleDTO>>
{
    public string CacheKey => $"work-order:{ScheduleDate:yyyy-MM-dd}:labor={LaborId?.ToString() ?? "-"}";
    public string[] Tags => ["work-order"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}