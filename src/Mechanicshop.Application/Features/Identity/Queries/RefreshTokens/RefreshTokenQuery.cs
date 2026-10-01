using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.Identity.Queries.RefreshTokens;

public record RefreshTokenQuery(string RefreshToken, string ExpiredAccessToken) : IRequest<Result<TokenResponse>>;