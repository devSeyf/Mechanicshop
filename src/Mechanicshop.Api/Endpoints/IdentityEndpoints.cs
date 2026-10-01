using Mechanicshop.Api.Extensions;
using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Identity;
using Mechanicshop.Application.Features.Identity.DTOs;
using Mechanicshop.Application.Features.Identity.Queries.GenerateTokens;
using Mechanicshop.Application.Features.Identity.Queries.GetUserInfo;
using Mechanicshop.Application.Features.Identity.Queries.RefreshTokens;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Mechanicshop.Api.Endpoints;

public static class IdentityEndpoints
{
    public static void MapTokenEndpoints(this IEndpointRouteBuilder app, Asp.Versioning.Builder.ApiVersionSet apiVersionSet)
    {
        var endpoints = app.MapGroup("/identity");

        endpoints.MapPost("/token/generate", GenerateToken)
            .WithName("GenerateToken")
            .WithSummary("Generates a new JWT access and refresh token.")
            .WithDescription("Authenticates a user with credentials and returns a token pair.")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        endpoints.MapPost("/token/refresh-token", RefreshToken)
            .WithName("RefreshToken")
            .WithSummary("Refreshes the JWT access token.")
            .WithDescription("Uses a valid refresh token to obtain a new access token.")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        endpoints.MapGet("/current-user/claims", GetUserInfo)
            .WithName("GetCurrentUserClaims")
            .RequireAuthorization()
            .WithSummary("Gets current authenticated user info.")
            .WithDescription("Returns user details extracted from the current JWT access token.")
            .Produces<AppUserDTO>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> RefreshToken(ISender sender, RefreshTokenQuery request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);

        return result.Match(
            value => Results.Ok(value),
            error => error.ToProblem());
    }

    private static async Task<IResult> GenerateToken(ISender sender, GenerateTokenQuery request, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(request);

        var result = await sender.Send(request, ct);

        return result.Match(
            value => Results.Ok(value),
            error => error.ToProblem());
    }

    private static async Task<IResult> GetUserInfo(ISender sender, IUser user, CancellationToken ct)
    {
        var result = await sender.Send(new GetUserByIdQuery(user.Id), ct);

        return result.Match(
                  value => Results.Ok(value),
                  error => error.ToProblem());
    }
}