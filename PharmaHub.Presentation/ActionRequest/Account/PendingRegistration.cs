using PharmaHub.Domain.Enums;

namespace PharmaHub.Presentation.ActionRequest.Account;

public class VerificationResponse
{
    public string Message { get; set; }
    public bool RequiresVerification { get; set; }
}

public class PendingRegistration
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string PharmacyName { get; set; }
    public string FormalPapersURL { get; set; }
    public string LogoURL { get; set; }
    public string PhoneNumber { get; set; }
    public CountriesSupported Country { get; set; }
    public Governorates City { get; set; }
    public string Address { get; set; }
    public byte OpenTime { get; set; }
    public byte CloseTime { get; set; }
    public DateTime DateRequested { get; set; }
    public string CreditCardNumber { get; set; }
}
