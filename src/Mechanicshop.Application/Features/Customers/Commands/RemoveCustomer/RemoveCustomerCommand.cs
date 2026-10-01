using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Customers.Commands.RemoveCustomer;

public sealed record RemoveCustomerCommand(Guid CustomerId) 
: IRequest<Result<Deleted>>;
