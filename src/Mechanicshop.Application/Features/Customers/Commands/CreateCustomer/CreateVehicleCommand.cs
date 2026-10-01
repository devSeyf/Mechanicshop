using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateVehicleCommand(
 string Make,
 string Model,
 int Year,
 string LicensePlate) : IRequest<Result<VehicleDTO>>;