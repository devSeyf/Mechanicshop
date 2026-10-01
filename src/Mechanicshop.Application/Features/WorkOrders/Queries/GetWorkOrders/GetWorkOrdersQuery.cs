using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Common.Models;
using Mechanicshop.Application.Features.WorkOrders.DTOs;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder.Enums;

namespace Mechanicshop.Application.Features.WorkOrders.Queries.GetWorkOrders;

public sealed record GetWorkOrdersQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    string SortColumn = "createdAt",
    string SortDirection = "desc",
    WorkOrderState? State = null,
    Guid? VehicleId = null,
    Guid? LaborId = null,
    DateTime? StartDateFrom = null,
    DateTime? StartDateTo = null,
    DateTime? EndDateFrom = null,
    DateTime? EndDateTo = null,
    Spot? Spot = null
) : ICachedQuery<Result<PaginatedList<WorkOrderListItemDTO>>>
{
    public string CacheKey =>
        $"work-orders:p={Page}:ps={PageSize}" +
        $":q={SearchTerm ?? "-"}" +
        $":sort={SortColumn}:{SortDirection}" +
        $":state={State?.ToString() ?? "-"}" +
        $":veh={VehicleId?.ToString() ?? "-"}" +
        $":lab={LaborId?.ToString() ?? "-"}" +
        $":sdfrom={StartDateFrom?.ToString("yyyyMMdd") ?? "-"}" +
        $":sdto={StartDateTo?.ToString("yyyyMMdd") ?? "-"}" +
        $":edfrom={EndDateFrom?.ToString("yyyyMMdd") ?? "-"}" +
        $":edto={EndDateTo?.ToString("yyyyMMdd") ?? "-"}" +
        $":spot={Spot?.ToString() ?? "-"}";

    public string[] Tags => ["work-order"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}