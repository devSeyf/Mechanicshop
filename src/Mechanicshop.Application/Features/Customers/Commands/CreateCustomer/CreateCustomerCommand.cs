using Mechanicshop.Application.Features.Customers.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Customers.Commands.CreateCustomer;


public sealed record CreateCustomerCommand(string Name,
                                           string PhoneNumber,
                                           string Email,
                                           List<CreateVehicleCommand> Vehicles)
: IRequest<Result<CustomerDTO>>;
