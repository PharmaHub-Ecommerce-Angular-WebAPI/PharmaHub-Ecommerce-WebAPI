using PharmaHub.DTOs.Authentication;
using PharmaHub.Presentation.ActionRequest.Account;

namespace PharmaHub.Presentation.Extensions;

public static class RegisterAccountExtensions
{
    public static CustomerDto ToDtoCustomer(this RegisterUserActionRequest newAccount)
          => new CustomerDto
          {
              CustomerName = newAccount.CustomerName,
              Password = newAccount.Password,
              ConfirmPassword = newAccount.ConfirmPassword,
              city = newAccount.City,
              Country = newAccount.Country,
              PhoneNumber = newAccount.PhoneNumber,
              Address = newAccount.Address,
          };
    public static PharmacyDto ToDtoPharmacy(this RegisterPharmacyActionRequest newAccount ,string uniqueFileName)
        => new PharmacyDto
        {
            PharmacyName = newAccount.PharmacyName,
            Password = newAccount.Password,
            ConfirmPassword = newAccount.ConfirmPassword,
            city = newAccount.city,
            OpenTime = newAccount.OpenTime,
            CloseTime = newAccount.CloseTime,
            FormalPapersURL = uniqueFileName,
            LogoURL = newAccount.LogoURL,
            CreditCardNumber = newAccount.CreditCardNumber,
            AccountStat =newAccount.AccountStat,
            Country = newAccount.Country,
            PhoneNumber = newAccount.PhoneNumber,
            Address = newAccount.Address,
        };

}
