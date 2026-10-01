using Asp.Versioning;
using Mechanicshop.Application.Features.Dashboard.DTOs;
using Mechanicshop.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mechanicshop.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
[Route("api/v{version:apiVersion}/dashboard")]
public sealed class DashboardController(ISender sender) : ApiController
{
    [HttpGet("stats")]
    [ProducesResponseType(typeof(TodayWorkOrderStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTodayStats([FromQuery] DateOnly? date, CancellationToken ct)
    {
        var statsDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var result = await sender.Send(new GetWorkOrderStatsQuery(statsDate), ct);

        return result.Match(
            response => Ok(response),
            Problem);
    }
}