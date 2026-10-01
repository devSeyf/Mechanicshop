using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Application.Features.Customers.Mappers;
using Mechanicshop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Customers.Queries.GetCustomerById;


public class GetCustomerByIdQueryHandler(
    ILogger<GetCustomerByIdQuery> logger,
    IAppDbContext context) : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDTO>>
{
    public async Task<Result<CustomerDTO>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.Include(c => c.Vehicles)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == query.CustomerId, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning("Customer with id {CustomerId} was not found", query.CustomerId);
            return Error.NotFound(code: "Customer_NotFound",
                description: $"Customer with id '{query.CustomerId}' was not found");
        }

        return customer.ToDTO();
    }
}