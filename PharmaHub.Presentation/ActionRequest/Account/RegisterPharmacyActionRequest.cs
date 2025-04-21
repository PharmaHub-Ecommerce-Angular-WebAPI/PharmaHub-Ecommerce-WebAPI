using PharmaHub.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PharmaHub.Presentation.ActionRequest.Account
{
    public class RegisterPharmacyActionRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Pharmacy name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Pharmacy name must be between 3 and 100 characters.")]
        public string PharmacyName { get; set; } = string.Empty;

        public AccountStats AccountStat { get; set; } = AccountStats.Pending;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [StrongPassword]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Phone number must be between 10 to 15 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Formal papers URL is required.")]
        [Url(ErrorMessage = "Formal papers URL must be a valid URL.")]
        public IFormFile FormalPapersURL { get; set; }

        [Required(ErrorMessage = "Logo URL is required.")]
        [Url(ErrorMessage = "Logo URL must be a valid URL.")]
        public IFormFile LogoURL { get; set; }

        [Required(ErrorMessage = "Credit card number is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Credit card number must be 16 digits.")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [EnumDataType(typeof(CountriesSupported), ErrorMessage = "Invalid country selection.")]
        public CountriesSupported Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [EnumDataType(typeof(Governorates), ErrorMessage = "Invalid city selection.")]
        public Governorates city { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Open time is required.")]
        [Range(0, 23, ErrorMessage = "Open time must be between 0 and 23.")]
        public byte OpenTime { get; set; }

        [Required(ErrorMessage = "Close time is required.")]
        [Range(0, 23, ErrorMessage = "Close time must be between 0 and 23.")]
        public byte CloseTime { get; set; }

       public string? VerificationCode { get; set; }
        public DateTime DateRequested { get; set; } = DateTime.UtcNow;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OpenTime >= CloseTime)
            {
                yield return new ValidationResult(
                    "Open time must be earlier than close time.",
                    new[] { nameof(OpenTime), nameof(CloseTime) }
                );
            }

            if (Country == default)
            {
                yield return new ValidationResult("You must select a valid country.", new[] { nameof(Country) });
            }

            if (city == default)
            {
                yield return new ValidationResult("You must select a valid city.", new[] { nameof(city) });
            }
        }
    }

}
