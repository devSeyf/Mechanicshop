using Mechanicshop.Application.Features.Billing.DTOs;
using Mechanicshop.Application.Features.Customers.Mappers;
using Mechanicshop.Domain.WorkOrder.Billing;
using MechanicShop.Application.Features.Billing.DTOs;

namespace Mechanicshop.Application.Features.Billing.Mappers;

public static class InvoiceMapper
{
    public static InvoiceDTO ToDto(this Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        return new InvoiceDTO
        {
            InvoiceId = invoice.Id,
            WorkOrderId = invoice.WorkOrderId,
            Customer = invoice.WorkOrder!.Vehicle!.Customer!.ToDTO(),
            Vehicle = invoice.WorkOrder.Vehicle.ToDTO(),
            IssuedAtUtc = invoice.IssuedAtUtc,
            Subtotal = invoice.Subtotal,
            TaxAmount = invoice.TaxAmount,
            DiscountAmount = invoice.DiscountAmount,
            Total = invoice.Total,
            PaymentStatus = invoice.Status.ToString(),
            Items = invoice.LineItems.Select(x => x.ToDto()).ToList()
        };
    }

    public static List<InvoiceDTO> ToDtos(this IEnumerable<Invoice> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }

    public static InvoiceLineItemDTO ToDto(this InvoiceLineItem item)
    {
        return new InvoiceLineItemDTO
        {
            InvoiceId = item.InvoiceId,
            LineNumber = item.LineNumber,
            Description = item.Description,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            LineTotal = item.LineTotal
        };
    }

    public static List<InvoiceLineItemDTO> ToDtos(this IEnumerable<InvoiceLineItem> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}