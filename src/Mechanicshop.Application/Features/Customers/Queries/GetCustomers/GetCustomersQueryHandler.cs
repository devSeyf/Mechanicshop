using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Application.Features.Customers.Mappers;
using Mechanicshop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mechanicshop.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(
    IAppDbContext context
    ) : IRequestHandler<GetCustomersQuery, Result<List<CustomerDTO>>>
{
    public async Task<Result<List<CustomerDTO>>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var customers = await context.Customers.Include(c => c.Vehicles).AsNoTracking().ToListAsync(cancellationToken);
        return customers.ToDTOs();
    }
}