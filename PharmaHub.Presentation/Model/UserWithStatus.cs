using PharmaHub.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PharmaHub.Presentation.Model;

public class UserWithStatus
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Account status is required.")]
    public AccountStats AccountStat { get; set; }

    [Required(ErrorMessage = "Roles are required.")]
    [MinLength(1, ErrorMessage = "At least one role must be specified.")]
    public IEnumerable<string> Roles { get; set; }
}

public class ValidRolesAttribute : ValidationAttribute
{
    private readonly string[] _allowedRoles = { "Admin", "Doctor", "Patient", "Pharmacist" };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IEnumerable<string> roles)
        {
            var invalidRoles = roles.Where(role => !_allowedRoles.Contains(role)).ToList();
            if (invalidRoles.Any())
            {
                return new ValidationResult($"Invalid roles: {string.Join(", ", invalidRoles)}. Allowed roles: {string.Join(", ", _allowedRoles)}");
            }
        }
        return ValidationResult.Success;
    }

}
