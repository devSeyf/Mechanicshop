using Mechanicshop.Application.Features.Billing.Mappers;
using Mechanicshop.Domain.WorkOrder.Billing;
using MechanicShop.Tests.Common.Billing;
using MechanicShop.Tests.Common.Customers;
using MechanicShop.Tests.Common.WorkOrders;
using Xunit;

namespace MechanicShop.Application.UnitTests.Mappers;

public class InvoiceMapperTest
{
    [Fact]
    public void InvoiceToDto_ShouldMapCorrectly()
    {
        // Arrange
        var customer = CustomerFactory.CreateCustomer().Value;
        var vehicle = customer.Vehicles.First();
        vehicle.Customer = customer;

        var workOrder = WorkOrderFactory.CreateWorkOrder(vehicle.Id, Guid.NewGuid()).Value;
        workOrder.Vehicle = vehicle;

        var invoiceLine = InvoiceLineItemFactory.CreateInvoiceLineItem(
            Guid.NewGuid(),
            1,
            "Oil Change",
            2,
            50).Value;

        var invoice = InvoiceFactory.CreateInvoice(
            Guid.NewGuid(),
            workOrder.Id,
            [invoiceLine],
            30m,
            20m).Value;

        invoice.WorkOrder = workOrder;

        // Act
        var dto = invoice.ToDto();

        // Assert
        Assert.Equal(invoice.Id, dto.InvoiceId);
        Assert.Equal(invoice.WorkOrderId, dto.WorkOrderId);
        Assert.Equal(invoice.TaxAmount, dto.TaxAmount);
        Assert.Equal(invoice.DiscountAmount, dto.DiscountAmount);

        Assert.Equivalent(invoice.LineItems.ToDtos(), dto.Items);

        Assert.Single(dto.Items);

        Assert.NotNull(dto.Customer);
        Assert.Equal(customer.Id, dto.Customer.CustomerId);
    }


}