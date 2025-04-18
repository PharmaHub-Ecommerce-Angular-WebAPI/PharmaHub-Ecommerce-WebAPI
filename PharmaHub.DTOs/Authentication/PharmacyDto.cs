using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.Authentication;

public class PharmacyDto
{
    public string PharmacyName { get; set; } = string.Empty;
    public AccountStats AccountStat { get; set; } = AccountStats.Pending; //Pending by default`
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FormalPapersURL { get; set; }
    public string LogoURL { get; set; }
    public string CreditCardNumber { get; set; }
    public CountriesSupported Country { get; set; } 
    public Governorates city { get; set; }
    public byte OpenTime { get; set; } 
    public byte CloseTime { get; set; }
    public string Address { get; set; } = string.Empty;
}
public static class RegisterPharmacyDtoExtensions
{
    public static Pharmacy ToEntity(this PharmacyDto dto)
        => new Pharmacy
        {
            UserName = dto.PharmacyName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.Password,
            Country = dto.Country,
            city = dto.city,
            AccountStat = dto.AccountStat,
            FormalPapersURL = dto.FormalPapersURL,
            LogoURL = dto.LogoURL,
            CreditCardNumber = dto.CreditCardNumber ,
            OpenTime = dto.OpenTime,
            CloseTime = dto.CloseTime,
            Address = dto.Address

        };


}
