using Mechanicshop.Application.Features.Identity.DTOs;
using Mechanicshop.Domain.Common.Results;

using MediatR;

namespace Mechanicshop.Application.Features.Identity.Queries.GetUserInfo;

public sealed record GetUserByIdQuery(string? UserId) : IRequest<Result<AppUserDTO>>;