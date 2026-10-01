using Mechanicshop.Application.Features.Billing.DTOs;
using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.Billing.Queries.GetInvoicePdf;

public sealed record GetInvoicePdfQuery(Guid InvoiceId) : IRequest<Result<InvoicePdfDTO>>
{
}