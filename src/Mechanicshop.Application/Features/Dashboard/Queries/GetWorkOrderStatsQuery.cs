using Mechanicshop.Application.Features.Dashboard.DTOs;
using Mechanicshop.Domain.Common.Results;
using MediatR;

namespace Mechanicshop.Application.Features.Dashboard.Queries;

public sealed record GetWorkOrderStatsQuery(DateOnly Date) : IRequest<Result<TodayWorkOrderStatsDto>>;