using PharmaHub.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PharmaHub.Presentation.ActionRequest.Account;

public class RegisterUserActionRequest : IValidatableObject
{
    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Customer name must be between 3 and 100 characters.")]
    public string CustomerName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Phone number must be between 10 to 15 digits and may start with '+'.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
    [DataType(DataType.Password)]
    [StrongPassword]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    [EnumDataType(typeof(CountriesSupported), ErrorMessage = "Invalid country selection.")]
    public CountriesSupported Country { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [EnumDataType(typeof(Governorates), ErrorMessage = "Invalid governorate selection.")]
    public Governorates City { get; set; }

    public string VerificationCode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {  
        // For country validation
        if (Country == 0) // default value for enum
        {
            yield return new ValidationResult("Country must be selected.", new[] { nameof(Country) });
        }
        else if (!Enum.IsDefined(typeof(CountriesSupported), Country))
        {
            yield return new ValidationResult("Invalid country selection.", new[] { nameof(Country) });
        }
        if (string.IsNullOrWhiteSpace(City.ToString()))
        {
            yield return new ValidationResult("City must be selected.", new[] { nameof(City) });
        }
        else if (!Enum.IsDefined(typeof(Governorates), City))
        {
            yield return new ValidationResult("Invalid city selection.", new[] { nameof(City) });
        }

    }
}

public class StrongPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrWhiteSpace(password))
            return new ValidationResult("Password is required.");

        var hasUpperChar = Regex.IsMatch(password, @"[A-Z]");
        var hasLowerChar = Regex.IsMatch(password, @"[a-z]");
        var hasNumber = Regex.IsMatch(password, @"[0-9]");
        var hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

        if (!hasUpperChar || !hasLowerChar || !hasNumber || !hasSpecialChar)
        {
            return new ValidationResult("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }

        return ValidationResult.Success;
    }
}
