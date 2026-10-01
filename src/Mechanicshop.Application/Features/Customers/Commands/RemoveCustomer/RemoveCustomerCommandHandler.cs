using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Customers.Commands.RemoveCustomer;

public class RemoveCustomerCommandHandler(
    ILogger<RemoveCustomerCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache) : IRequestHandler<RemoveCustomerCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(RemoveCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await context.Customers
       .FindAsync([command.CustomerId], cancellationToken);

        if (customer is null)
        {
            logger.LogWarning("Customer with id {CustomerId} not found for deletion.", command.CustomerId);
            return ApplicationErrors.CustomerNotFound;
        }

        var hasAssociatedWorkOrders = await context.WorkOrders
        .Where(wo => wo.Vehicle != null)
        .AnyAsync(wo => wo.Vehicle!.CustomerId == command.CustomerId, cancellationToken);

        if (hasAssociatedWorkOrders)
        {
            logger.LogWarning("Customer {CustomerId} cannot be deleted because they have associated work orders (past, scheduled, or in-progress).", command.CustomerId);
            return CustomerErrors.CannotDeleteCustomerWithWorkOrders;
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"customer", cancellationToken);
        logger.LogInformation("Customer {CustomerId} deleted successfully.", command.CustomerId);
        return Result.Deleted;
    }
}