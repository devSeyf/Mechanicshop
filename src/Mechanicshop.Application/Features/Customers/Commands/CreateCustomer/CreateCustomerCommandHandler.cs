using System.ComponentModel;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Application.Features.Customers.Mappers;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.Customers;
using Mechanicshop.Domain.Customers.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler(IAppDbContext context,
                                                 ILogger<CreateCustomerCommandHandler> logger,
                                                 HybridCache cache)
: IRequestHandler<CreateCustomerCommand, Result<CustomerDTO>>
{
    public async Task<Result<CustomerDTO>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLower();

        var existing = await context.Customers.AnyAsync(c =>
            c.Email!.Trim().ToLower() == email, cancellationToken
        );
        if (existing)
        {
            logger.LogWarning("customer creation aborted. Email already exists");
            return CustomerErrors.CustomerExists;
        }

        List<Vehicle> vehicles = [];

        foreach (var v in command.Vehicles)
        {
            var vehicleResult = Vehicle.Create(Guid.NewGuid(), v.Make, v.Model, v.Year, v.LicensePlate);
            if (vehicleResult.IsError)
            {
                return vehicleResult.Errors;
            }
            vehicles.Add(vehicleResult.Value);
        }

        var createCustomerResult = Customer.Create(
            Guid.NewGuid(),
            command.Name.Trim(),
            command.PhoneNumber.Trim(),
            command.Email.Trim(),
            vehicles
        );

        if (createCustomerResult.IsError)
        {
            return createCustomerResult.Errors;
        }

        context.Customers.Add(createCustomerResult.Value);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync("customer", cancellationToken);

        logger.LogInformation("Customer created successfully. Id: {CustomerId}", createCustomerResult.Value.Id);

        return createCustomerResult.Value.ToDTO();
    }
}