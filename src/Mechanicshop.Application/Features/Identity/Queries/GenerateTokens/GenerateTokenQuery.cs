using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.Identity.Queries.GenerateTokens;

public record GenerateTokenQuery(
    string Email,
    string Password) : IRequest<Result<TokenResponse>>;