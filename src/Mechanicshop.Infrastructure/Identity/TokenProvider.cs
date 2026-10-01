using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Identity;
using Mechanicshop.Application.Features.Identity.DTOs;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Mechanicshop.Infrastructure.Identity;

public class TokenProvider(IConfiguration configuration, IAppDbContext context) : ITokenProvider
{
    public async Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDTO user, CancellationToken ct = default)
    {
        var tokenResult = await CreateAsync(user, ct);

        if (tokenResult.IsError)
        {
            return tokenResult.Errors;
        }

        return tokenResult.Value;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
            ValidateIssuer = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["JwtSettings:Audience"],
            ValidateLifetime = false, // Ignore token expiration
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token.");
        }

        return principal;
    }

    private async Task<Result<TokenResponse>> CreateAsync(AppUserDTO user, CancellationToken ct = default)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var issuer = jwtSettings["Issuer"]!;
        var audience = jwtSettings["Audience"]!;
        var key = jwtSettings["Secret"]!;

        var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["TokenExpirationInMinutes"]!));

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.UserId!),
            new (JwtRegisteredClaimNames.Email, user.Email!),
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(descriptor);

        var oldRefreshTokens = await context.RefreshTokens
              .Where(rt => rt.UserId == user.UserId)
              .ExecuteDeleteAsync(ct);

        var refreshTokenResult = RefreshToken.Create(
            Guid.NewGuid(),
            GenerateRefreshToken(),
            user.UserId,
            DateTime.UtcNow.AddDays(7));

        if (refreshTokenResult.IsError)
        {
            return refreshTokenResult.Errors;
        }

        var refreshToken = refreshTokenResult.Value;

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(ct);

        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(securityToken),
            RefreshToken = refreshToken.Token,
            ExpiresOnUtc = expires
        };
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}