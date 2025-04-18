using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.Presentation.ActionRequest.Account;
using PharmaHub.Service.JWT_Handler;
using PharmaHub.Service.UserHandler;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;
        private readonly JwtTokenService _jwtService;
        private readonly ISmsService _smsService;
        private static readonly Dictionary<string, string> _phoneVerificationCodes = new Dictionary<string, string>();

        public AccountController(
            UserManager<User> userManager, 
            IConfiguration config,
            IFileService fileService, 
            JwtTokenService jwtTokenService,
            ISmsService smsService
            )
        {
            _userManager = userManager;
            _config = config;
            _fileService=fileService;
            _jwtService=jwtTokenService;
            _smsService = smsService;
        }
        #region Phone Verification

        [HttpPost("send-code")]
        public async Task<IActionResult> SendVerificationCode([FromBody] PhoneVerificationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return BadRequest("Phone number is required.");

            // Check if phone already registered
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingUser != null)
                return BadRequest("This phone number is already registered.");

            await _smsService.SendVerificationCodeAsync(request.PhoneNumber);
            return Ok("Verification code sent.");
        } 
        #endregion


        #region Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserActionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check phone not already in use
            var existingUserWithPhone = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingUserWithPhone != null)
                return BadRequest(new { Message = "This phone number is already registered." });

            // Check if email already exists
            var existingUserWithEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserWithEmail != null)
            {
                return BadRequest("Email is already registered.");
            }

            // ✅ Verify phone code
            var isCodeValid = _smsService.VerifyPhoneCode(request.PhoneNumber, request.VerificationCode);
            if (!isCodeValid)
                return BadRequest("Invalid or expired verification code.");

            var user = MapToCustomer(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Clean up verification code after successful registration
                _phoneVerificationCodes.Remove(request.PhoneNumber);
                return Ok("Account Registered");
            }
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return BadRequest(errors);
        }
        #endregion

        #region RegisterPharmacy
        [HttpPost("pharmacyregister")]
        public async Task<IActionResult> RegisterPharmacy([FromBody] RegisterPharmacyActionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uniqueFileName = _fileService.UploadFile(request.FormalPapersURL, "pdf");
            var pharmacy = MapToPharmacy(request, uniqueFileName);

            var result = await _userManager.CreateAsync(pharmacy, request.Password);

            if (result.Succeeded)
            {
                return Ok("Pharmacy account registered successfully. Pending admin approval.");
            }


            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(errors);
        }

        [HttpPut("approve/{pharmacyId}")]
        public async Task<IActionResult> ApprovePharmacy(string pharmacyId)
        {
            var pharmacy = await _userManager.FindByIdAsync(pharmacyId);
            if (pharmacy == null || pharmacy is not Pharmacy)
                return NotFound("Pharmacy not found.");

            ((Pharmacy)pharmacy).AccountStat = AccountStats.Active;

            var updateResult = await _userManager.UpdateAsync(pharmacy);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(pharmacy, "Pharmacy");

            return Ok("Pharmacy approved and role assigned.");
        }

        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserActionRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);

            // User exists
            if (user != null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

                // Password Valid
                if (isPasswordValid)
                {
                    // Generate token using JwtTokenService
                    var token = await _jwtService.GenerateToken(user);

                    return Ok(new
                    {
                        token = token,
                        message = "Login successful"
                    });
                }
            }

            // User does not exist or invalid password
            return Unauthorized(new { message = "Invalid username or password" });
        }
        #endregion

        #region Mapping method

        private Pharmacy MapToPharmacy(RegisterPharmacyActionRequest request, string uniqueFileName)
        {
            return new Pharmacy
            {
                UserName = request.PharmacyName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Country = request.Country,
                city = request.city,
                AccountStat = request.AccountStat,
                FormalPapersURL = uniqueFileName,
                LogoURL = request.LogoURL,
                CreditCardNumber = request.CreditCardNumber,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,

            };
        }
        private Customer MapToCustomer(RegisterUserActionRequest request)
        {
            return new Customer
            {
                UserName = request.CustomerName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Country = request.Country,
                city = request.City,
                Address =request.Address
            };
        }
        #endregion

    }


}


