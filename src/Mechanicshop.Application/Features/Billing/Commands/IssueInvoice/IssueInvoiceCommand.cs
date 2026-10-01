using Mechanicshop.Application.Features.Billing.DTOs;
using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.Billing.Commands.IssueInvoice;

public sealed record IssueInvoiceCommand(Guid WorkOrderId) : IRequest<Result<InvoiceDTO>>;