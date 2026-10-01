using System.Security.Claims;

using Mechanicshop.Application.Features.Identity;
using Mechanicshop.Application.Features.Identity.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDTO user, CancellationToken ct = default);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}