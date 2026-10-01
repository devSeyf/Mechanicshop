using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Application.Features.RepairTasks.Mappers;
using Mechanicshop.Application.Features.RepairTasks.Queries.GetRepairTaskById;
using Mechanicshop.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTaskById;

public class GetRepairTaskByIdQueryHandler(
    ILogger<GetRepairTaskByIdQueryHandler> logger,
    IAppDbContext context
    )
    : IRequestHandler<GetRepairTaskByIdQuery, Result<RepairTaskDTO>>
{
    public async Task<Result<RepairTaskDTO>> Handle(GetRepairTaskByIdQuery query, CancellationToken ct)
    {
        var repairTask = await context.RepairTasks.AsNoTracking().Include(c => c.Parts)
                                     .FirstOrDefaultAsync(c => c.Id == query.RepairTaskId, ct);

        if (repairTask is null)
        {
            logger.LogWarning("Repair task with id {RepairTaskId} was not found", query.RepairTaskId);

            return ApplicationErrors.RepairTaskNotFound;
        }

        return repairTask.ToDto();
    }
}