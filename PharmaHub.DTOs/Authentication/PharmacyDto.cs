using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.Authentication;

public class PharmacyDto
{
    public PharmacyDto(Pharmacy ph)
    {
        PharmacyName = ph.PharmacyName;
        Email = ph.Email;
        PhoneNumber = ph.PhoneNumber;
        FormalPapersURL = ph.FormalPapersURL;
        LogoURL = ph.LogoURL;
        CreditCardNumber = ph.CreditCardNumber;
        Country = ph.Country;
        city = ph.city;
        Address = ph.Address;
        OpenTime = ph.OpenTime;
        CloseTime = ph.CloseTime;

    }
    public string PharmacyName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FormalPapersURL { get; set; }
    public string LogoURL { get; set; }
    public string CreditCardNumber { get; set; }
    public CountriesSupported Country { get; set; } 
    public Governorates city { get; set; }
    public string Address { get; set; }
    public byte OpenTime { get; set; } 
    public byte CloseTime { get; set; } 
}
