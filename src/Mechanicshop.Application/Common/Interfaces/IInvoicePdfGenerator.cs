using Mechanicshop.Domain.WorkOrder.Billing;

namespace Mechanicshop.Application.Common.Interfaces;

public interface IInvoicePdfGenerator
{
    byte[] Generate(Invoice invoice);
}