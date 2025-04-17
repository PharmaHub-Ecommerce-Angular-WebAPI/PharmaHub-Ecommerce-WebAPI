using Microsoft.AspNetCore.Identity;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.DTOs.Authentication;

namespace PharmaHub.Service.Services;

public interface IUserService
{

    #region Customer
    Task<IdentityResult> RegisterCustomer(CustomerDto user);
    #endregion

    #region Pharmacy
    Task<IdentityResult> Registerpharmacy(PharmacyDto user);
    #endregion

    Task Signin(User User, bool IsCookiePersistant);
    Task<User?> FindUserByUserName(string UserName);
    Task<bool> CheckPassword(User user, string Password);
    Task Signout();

}
