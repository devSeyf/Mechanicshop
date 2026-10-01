using Mechanicshop.Application.Features.Identity.DTOs;
using Mechanicshop.Domain.Common.Results;

namespace Mechanicshop.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string? policyName);

    Task<Result<AppUserDTO>> AuthenticateAsync(string email, string password);

    Task<Result<AppUserDTO>> GetUserByIdAsync(string userId);

    Task<string?> GetUserNameAsync(string userId);
}