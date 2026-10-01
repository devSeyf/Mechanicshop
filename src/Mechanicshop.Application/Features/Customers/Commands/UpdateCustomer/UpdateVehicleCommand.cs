using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Customers.Commands.UpdateCustomer;

public sealed record UpdateVehicleCommand(Guid? VehicleId,
    string Make,
    string Model,
    int Year,
    string LicensePlate) : IRequest<Result<VehicleDTO>>;