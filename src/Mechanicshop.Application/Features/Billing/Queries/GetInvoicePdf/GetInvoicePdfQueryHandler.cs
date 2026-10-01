using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Billing.DTOs;
using Mechanicshop.Domain.Common.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Billing.Queries.GetInvoicePdf;

public class GetInvoicePdfQueryHandler(
    ILogger<GetInvoicePdfQueryHandler> logger,
    IInvoicePdfGenerator pdfGenerator,
    IAppDbContext context
    )
    : IRequestHandler<GetInvoicePdfQuery, Result<InvoicePdfDTO>>
{
    public async Task<Result<InvoicePdfDTO>> Handle(GetInvoicePdfQuery query, CancellationToken ct)
    {
        var invoice = await context.Invoices.AsNoTracking()
           .Include(i => i.LineItems)
           .FirstOrDefaultAsync(i => i.Id == query.InvoiceId, ct);

        if (invoice is null)
        {
            logger.LogWarning("Invoice not found. InvoiceId: {InvoiceId}", query.InvoiceId);
            return Error.NotFound("Invoice not found.");
        }

        try
        {
            var pdfBytes = pdfGenerator.Generate(invoice);

            var invoicePdf = new InvoicePdfDTO
            {
                Content = pdfBytes,
                FileName = $"invoice-{invoice.Id}.pdf"
            };

            return invoicePdf;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate PDF for InvoiceId: {InvoiceId}", query.InvoiceId);
            return Error.Failure("An error occurred while generating the invoice PDF.");
        }
    }
}