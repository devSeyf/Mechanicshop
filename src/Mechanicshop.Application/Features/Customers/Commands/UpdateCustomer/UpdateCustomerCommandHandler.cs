using Mechanicshop.Application.Common.Errors;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.Customers.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(
    ILogger<UpdateCustomerCommand> logger,
    IAppDbContext context,
    HybridCache cache) : IRequestHandler<UpdateCustomerCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.Include(c => c.Vehicles)
        .FirstOrDefaultAsync(c => c.Id == command.CustomerId, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning("Customer {CustomerId} not found for update.", command.CustomerId);
            return ApplicationErrors.CustomerNotFound;
        }

        var validatedVehicles = new List<Vehicle>();
        foreach (var v in command.Vehicles)
        {
            var vehicleId = v.VehicleId ?? Guid.NewGuid();

            var vehicleResult = Vehicle.Create(vehicleId, v.Make, v.Model, v.Year, v.LicensePlate);
            if (vehicleResult.IsError)
            {
                return vehicleResult.Errors;
            }
            validatedVehicles.Add(vehicleResult.Value);
        }

        var updateCustomerResult = customer.Update(command.Name, command.Email, command.PhoneNumber);

        if (updateCustomerResult.IsError)
        {
            return updateCustomerResult.Errors;
        }

        var upsertPartsResult = customer.UpsertParts(validatedVehicles);

        if (upsertPartsResult.IsError)
        {
            return upsertPartsResult.Errors;
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"customer", cancellationToken);

        return Result.Updated;
    }
}