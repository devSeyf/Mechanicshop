using Mechanicshop.Api.Extensions;
using Mechanicshop.Application.Features.Labors.DTOs;
using Mechanicshop.Application.Features.Labors.Queries.GetLabors;
using Mechanicshop.Domain.Identity;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Mechanicshop.Api.Endpoints;

public static class LaborEndpoints
{
    public static void MapLaborEndpoints(this IEndpointRouteBuilder app, Asp.Versioning.Builder.ApiVersionSet apiVersionSet)
    {
        var endpoints = app.MapGroup("/api/v{apiVersion:apiVersion}/labors")
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(Role.Manager.ToString()));

        endpoints.MapGet("/", GetLabors)
            .WithName("GetLabors")
            .MapToApiVersion(1.0)
            .WithSummary("Retrieves all labor definitions.")
            .WithDescription("Returns a list of available labor types that can be assigned to work orders.")
            .Produces<List<LaborDTO>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> GetLabors(ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetLaborsQuery(), ct);

        return result.Match(
         value => Results.Ok(value),
         error => error.ToProblem());
    }
}