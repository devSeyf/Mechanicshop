using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Labors.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Features.Labors.Queries.GetLabors;

public sealed record GetLaborsQuery : ICachedQuery<Result<List<LaborDTO>>>
{
    public string CacheKey => $"labors";
    public string[] Tags => ["labors"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}