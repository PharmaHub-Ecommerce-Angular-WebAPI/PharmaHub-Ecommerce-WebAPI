using System.ComponentModel.DataAnnotations;

namespace PharmaHub.Presentation.ActionRequest.Account;

public class LoginUserActionRequest
{

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "Remember me option is required.")]
    public bool RememberMe { get; set; } = false;
}
