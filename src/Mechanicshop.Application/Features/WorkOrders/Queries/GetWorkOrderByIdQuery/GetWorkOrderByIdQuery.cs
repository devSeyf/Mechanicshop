using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.WorkOrders.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : ICachedQuery<Result<WorkOrderDTO>>
{
    public string CacheKey => $"work-order:{WorkOrderId}";
    public string[] Tags => ["work-order"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}