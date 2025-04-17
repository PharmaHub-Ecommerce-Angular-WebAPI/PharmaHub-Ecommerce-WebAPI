using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DTOs.Authentication;

public class LoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
   
}
public static class LoginUserDtoExtensions
{
    public static User ToEntity(this LoginDTO dto)
        => new User
        {
            Email = dto.Email,
            PasswordHash = dto.Password,
        };

}
