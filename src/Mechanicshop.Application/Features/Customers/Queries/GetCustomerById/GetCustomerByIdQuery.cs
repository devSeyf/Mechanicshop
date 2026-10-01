using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : ICachedQuery<Result<CustomerDTO>>
{
    public string CacheKey => $"customer_{CustomerId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["customer"];
}