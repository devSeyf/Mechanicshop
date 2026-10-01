using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.Identity.DTOs;
using Mechanicshop.Domain.Common.Results;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.Identity.Queries.GetUserInfo;

public class GetUserByIdQueryHanlder(ILogger<GetUserByIdQueryHanlder> logger, IIdentityService identityService)
    : IRequestHandler<GetUserByIdQuery, Result<AppUserDTO>>
{
    public async Task<Result<AppUserDTO>> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var getUserByIdResult = await identityService.GetUserByIdAsync(request.UserId!);

        if (getUserByIdResult.IsError)
        {
            logger.LogError("User with Id { UserId }{ErrorDetails}", request.UserId, getUserByIdResult.TopError.Description);

            return getUserByIdResult.Errors;
        }

        return getUserByIdResult.Value;
    }
}