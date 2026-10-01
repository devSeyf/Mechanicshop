using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery : ICachedQuery<Result<List<CustomerDTO>>>
{
    public string CacheKey => "customers";
    public string[] Tags => ["customer"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
