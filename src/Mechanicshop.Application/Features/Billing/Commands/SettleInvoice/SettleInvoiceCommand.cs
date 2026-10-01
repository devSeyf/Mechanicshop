using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.Billing.Commands.SettleInvoice;

public sealed record SettleInvoiceCommand(Guid InvoiceId) : IRequest<Result<Success>>;