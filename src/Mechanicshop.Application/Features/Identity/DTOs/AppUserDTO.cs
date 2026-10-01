using System.Security.Claims;

namespace Mechanicshop.Application.Features.Identity.DTOs;

public sealed record AppUserDTO(string UserId, string Email, IList<string> Roles, IList<Claim> Claims);