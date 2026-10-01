using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.WorkOrders.DTOs;
using Mechanicshop.Application.Features.WorkOrders.Mappers;
using Mechanicshop.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public class GetWorkOrderByIdQueryHandler(
    ILogger<GetWorkOrderByIdQueryHandler> logger,
    IAppDbContext context
    )
    : IRequestHandler<GetWorkOrderByIdQuery, Result<WorkOrderDTO>>
{
    public async Task<Result<WorkOrderDTO>> Handle(GetWorkOrderByIdQuery query, CancellationToken ct)
    {
        var workOrder = await context.WorkOrders.AsNoTracking()
                                            .Include(a => a.RepairTasks)
                                               .ThenInclude(a => a.Parts)
                                            .Include(a => a.Labor)
                                            .Include(a => a.Vehicle!)
                                                .ThenInclude(v => v.Customer)
                                            .Include(a => a.Invoice)
                                        .FirstOrDefaultAsync(a => a.Id == query.WorkOrderId, ct);

        if (workOrder is null)
        {
            logger.LogWarning("WorkOrder with id {WorkOrderId} was not found", query.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        return workOrder.ToDto();
    }
}