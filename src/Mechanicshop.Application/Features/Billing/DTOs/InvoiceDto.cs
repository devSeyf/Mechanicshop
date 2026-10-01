using Mechanicshop.Application.Features.Customers.DTOs;
using MechanicShop.Application.Features.Billing.DTOs;

namespace Mechanicshop.Application.Features.Billing.DTOs;

public class InvoiceDTO
{
    public Guid InvoiceId { get; set; }
    public Guid WorkOrderId { get; set; }
    public DateTimeOffset IssuedAtUtc { get; set; }
    public CustomerDTO? Customer { get; set; }
    public VehicleDTO? Vehicle { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }
    public string? PaymentStatus { get; set; }

    public List<InvoiceLineItemDTO> Items { get; set; } = [];
}