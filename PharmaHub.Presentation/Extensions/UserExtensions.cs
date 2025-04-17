using PharmaHub.DTOs.Authentication;
using PharmaHub.Presentation.ActionRequest.Account;

namespace PharmaHub.Presentation.Extensions;

public static class UserExtensions
{
    public static LoginDTO ToDto(this LoginUserActionRequest request)
        => new LoginDTO
        {
            Email = request.Email,
            Password = request.Password,
        };
}
