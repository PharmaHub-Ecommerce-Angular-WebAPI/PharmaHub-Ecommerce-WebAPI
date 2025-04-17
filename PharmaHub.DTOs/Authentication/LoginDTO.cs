using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DTOs.Authentication;

public class LoginDTO
{
    public LoginDTO(User user)
    {
        user.Email = Email;
        user.PasswordHash = Password ;
    }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
