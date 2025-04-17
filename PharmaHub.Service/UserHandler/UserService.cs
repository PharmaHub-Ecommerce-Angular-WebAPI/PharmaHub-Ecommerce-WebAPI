using Microsoft.AspNetCore.Identity;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.DTOs.Authentication;
using PharmaHub.Business.Extensions;
using NETCore.MailKit.Core;

namespace PharmaHub.Service.UserHandler;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly VerificationCodeStore _codeStore;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;

    public UserService(
     UserManager<User> userManager,
     SignInManager<User> signInManager,
     VerificationCodeStore codeStore,
     IEmailService emailService,
     ISmsService smsService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _codeStore = codeStore;
        _emailService = emailService;
        _smsService = smsService;
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

    public async Task SendVerificationCodeAsync(User user)
    {
        var code = new Random().Next(100000, 999999).ToString();
        _codeStore.Store(user.Id, code);

        var message = $"Your PharmaHub verification code is: {code}";
        await _emailService.SendAsync(user.Email, "Verify Your Email", message);
        await _smsService.SendSmsAsync(user.PhoneNumber, message);
    }

    public async Task<bool> VerifyCodeAsync(string userId, string enteredCode)
    {
        var result = _codeStore.Verify(userId, enteredCode);
        if (!result) return false;

        var user = await _userManager.FindByIdAsync(userId);
        user.EmailConfirmed = true;
        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);

        return true;
    }

}