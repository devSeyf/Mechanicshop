using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Application.Features.RepairTasks.Mappers;
using Mechanicshop.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Mechanicshop.Application.Features.RepairTasks.Queries.GetRepairTasks;

public class GetRepairTasksQueryHandler(IAppDbContext context)
    : IRequestHandler<GetRepairTasksQuery, Result<List<RepairTaskDTO>>>
{
    public async Task<Result<List<RepairTaskDTO>>> Handle(GetRepairTasksQuery query, CancellationToken ct)
    {
        var repairTasks = await context.RepairTasks.Include(rt => rt.Parts).AsNoTracking().ToListAsync(ct);

        return repairTasks.ToDtos();
    }
}