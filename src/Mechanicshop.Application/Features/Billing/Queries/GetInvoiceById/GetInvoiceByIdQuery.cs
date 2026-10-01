using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Billing.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Features.Billing.Queries.GetInvoiceById;

public sealed record GetInvoiceByIdQuery(Guid InvoiceId) : ICachedQuery<Result<InvoiceDTO>>
{
    public string CacheKey => $"invoice_{InvoiceId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["invoice"];
}