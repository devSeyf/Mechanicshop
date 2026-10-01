using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Labors.DTOs;
using Mechanicshop.Application.Features.Labors.Mappers;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mechanicshop.Application.Features.Labors.Queries.GetLabors;

public class GetLaborsQueryHandler(IAppDbContext context) : IRequestHandler<GetLaborsQuery, Result<List<LaborDTO>>>
{
    public async Task<Result<List<LaborDTO>>> Handle(GetLaborsQuery request, CancellationToken cancellationToken)
    {
        var labors = await context.Employees.AsNoTracking().Where(e => e.Role == Role.Labor).ToListAsync(cancellationToken);

        return labors.ToDtos();
    }
}