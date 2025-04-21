using Microsoft.AspNetCore.Identity;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.DTOs.Authentication;
using PharmaHub.Business.Extensions;
using NETCore.MailKit.Core;
using PharmaHub.Service.UserHandler.Verification;

namespace PharmaHub.Service.UserHandler;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;



    public UserService(
     UserManager<User> userManager,
     SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
       
    }
    public async Task<bool> CheckPassword(User user, string Password)

      => await _userManager.CheckPasswordAsync(user, Password);
        
    public async Task<User?> FindUserByUserName(string UserName)

        => await _userManager.FindByNameAsync(UserName);
    

    public async Task<IdentityResult> RegisterCustomer(CustomerDto user)
    
       => await _userManager.CreateAsync(user.ToEntityCustomer(), user.Password);

    public async Task<IdentityResult> Registerpharmacy(PharmacyDto user)

       => await _userManager.CreateAsync(user.ToEntityPharmacy(), user.Password);

    public async Task Signin(User User, bool IsCookiePersistant)
    
        => await _signInManager.SignInAsync(User, IsCookiePersistant);
    

    public async Task Signout()
    
     =>   await _signInManager.SignOutAsync();



}