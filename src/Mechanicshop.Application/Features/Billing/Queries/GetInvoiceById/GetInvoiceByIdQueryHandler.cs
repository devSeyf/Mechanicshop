using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Billing.DTOs;
using Mechanicshop.Application.Features.Billing.Mappers;
using Mechanicshop.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Billing.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryHandler(
    ILogger<GetInvoiceByIdQueryHandler> logger,
    IAppDbContext context
    )
    : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDTO>>
{
    public async Task<Result<InvoiceDTO>> Handle(GetInvoiceByIdQuery query, CancellationToken ct)
    {
        var invoice = await context.Invoices.AsNoTracking()
            .Include(i => i.LineItems)
            .Include(i => i.WorkOrder!)
                .ThenInclude(w => w.Vehicle!)
                    .ThenInclude(v => v.Customer)
            .FirstOrDefaultAsync(i => i.Id == query.InvoiceId, ct);

        if (invoice is null)
        {
            logger.LogWarning("Invoice not found. InvoiceId: {InvoiceId}", query.InvoiceId);
            return Error.NotFound("Invoice not found.");
        }

        return invoice.ToDto();
    }
}