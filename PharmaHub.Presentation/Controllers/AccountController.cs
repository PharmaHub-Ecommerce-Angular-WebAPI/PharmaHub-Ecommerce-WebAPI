using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.Presentation.ActionRequest.Account;
using PharmaHub.Presentation.Extensions;
using PharmaHub.Service.JWT_Handler;
using PharmaHub.Service.UserHandler;
using PharmaHub.Service.UserHandler.Verification;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IFileService _fileService;
        private readonly JwtTokenService _jwtService;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IEmailService _emailService;


        public AccountController(
            UserManager<User> userManager, 
            IMemoryCache memoryCache,
            IFileService fileService, 
            JwtTokenService jwtTokenService,
            IVerificationCodeService verificationCodeService,
            IEmailService emailService
            )
        {
            _userManager = userManager;
           _memoryCache = memoryCache;
            _fileService=fileService;
            _jwtService=jwtTokenService;
           _verificationCodeService=verificationCodeService;
            _emailService=emailService;
        }


        
        #region Register with Built-in Email Verification
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserActionRequest request)
        {
            // Normalize email before validation
            request.Email = request.Email?.Trim().ToLower();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedEmail = request.Email;

            var existingUser = await _userManager.FindByEmailAsync(normalizedEmail);
            if (existingUser != null)
                return BadRequest("Email is already registered");

            // First step - no code provided
            if (string.IsNullOrEmpty(request.VerificationCode))
            {
                // ✅ Check attempts before generating code
                if (_verificationCodeService.HasTooManyAttempts(normalizedEmail))
                    return BadRequest("Too many attempts. Please try again later.");

                var code = _verificationCodeService.GenerateAndStoreCode(normalizedEmail);

                await _emailService.SendVerificationCode(normalizedEmail, code, request.CustomerName);

                return Ok(new
                {
                    Message = "Verification code sent to your email",
                    RequiresVerification = true
                });
            }
            else
            {
                var verificationResult = _verificationCodeService.VerifyCode(normalizedEmail, request.VerificationCode);

                if (verificationResult == "Invalid")
                    return BadRequest($"Invalid verification code. You have {_verificationCodeService.GetRemainingAttempts(normalizedEmail)} attempts remaining.");

                if (verificationResult == "Expired")
                    return BadRequest("Verification code has expired. Please request a new one.");


                var user = MapToCustomer(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                    await _userManager.AddToRoleAsync(user, "Customer");
                    _verificationCodeService.ClearCode(normalizedEmail);
                    return Ok("Account registered successfully");
            }

                return BadRequest(result.Errors.Select(e => e.Description));
        }
        }


        // Add this endpoint for code resend
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequest request)
        {
            var normalizedEmail = request.Email?.Trim().ToLower();

            // Generate new code
            var code = _verificationCodeService.GenerateAndStoreCode(normalizedEmail);

            // Send email
            await _emailService.SendVerificationCode(normalizedEmail, code, request.CustomerName);

            return Ok("New verification code sent");
        }

        #endregion

        #region RegisterPharmacy
        [HttpPost("pharmacyregister")]
        public async Task<IActionResult> RegisterPharmacy([FromForm] RegisterPharmacyActionRequest request)
        {
            // Normalize email before validation
            request.Email = request.Email?.Trim().ToLower();

            // Validate the file type
            if (request.FormalPapersURL is null)
                return BadRequest("Formal papers URL is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the email is already registered
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("Email is already registered");

            // First step - no code provided
            if (string.IsNullOrEmpty(request.VerificationCode))
            {
                // ✅ Check attempts before generating code
                if (_verificationCodeService.HasTooManyAttempts(request.Email))
                    return BadRequest("Too many attempts. Please try again later.");

                var code = _verificationCodeService.GenerateAndStoreCode(request.Email);

                await _emailService.SendVerificationCode(request.Email, code, request.PharmacyName);

                return Ok(new VerificationResponse
            {
                    Message = "Verification code sent to your email",
                    RequiresVerification = true
                });
            }
            else
            {
                var verificationResult = _verificationCodeService.VerifyCode(request.Email, request.VerificationCode);

                if (verificationResult == "Invalid")
                    return BadRequest($"Invalid verification code. You have {_verificationCodeService.GetRemainingAttempts(request.Email)} attempts remaining.");

                if (verificationResult == "Expired")
                    return BadRequest("Verification code has expired. Please request a new one.");

                var uploadedFileName = _fileService.UploadFile(request.FormalPapersURL, "FormalPapers", "pdf");
                var logoFileName = _fileService.UploadFile(request.LogoURL, "logo", "image");

                // Store only metadata in cache
                var pendingRegistration = new PendingRegistration
                {
                    Email = request.Email,
                    Password = request.Password,
                    LogoURL = logoFileName,
                    CreditCardNumber = request.CreditCardNumber,
                    PharmacyName = request.PharmacyName,
                    FormalPapersURL = uploadedFileName, // Store filename only
                    PhoneNumber = request.PhoneNumber,
                    Country = request.Country,
                    City = request.city,
                    Address = request.Address,
                    OpenTime = request.OpenTime,
                    CloseTime = request.CloseTime,
                    DateRequested = DateTime.UtcNow
                };

                _memoryCache.Set($"PendingPharmacy_{request.Email}", pendingRegistration, TimeSpan.FromDays(30));
                return Ok(new { Message = "Registration pending admin approval." });
            }
        }



        [HttpPut("approve/{email}")]
        public async Task<IActionResult> ApprovePharmacy(string email)
        {
            var cacheKey = $"PendingPharmacy_{email.ToLower()}";

            if (!_memoryCache.TryGetValue(cacheKey, out PendingRegistration pendingPharmacy))
                return NotFound("Pending registration not found or expired.");

            // Convert PendingRegistration to RegisterPharmacyActionRequest
            var registerRequest = ConversionHelper.ConvertToRegisterPharmacyActionRequest(pendingPharmacy);

            var uploadedFileName = pendingPharmacy.FormalPapersURL;
            var logoFileName = pendingPharmacy.LogoURL;

            // Map to Pharmacy entity
           var pharmacy = MapToPharmacy(registerRequest, uploadedFileName, logoFileName);

            // Create the pharmacy user
            var result = await _userManager.CreateAsync(pharmacy, pendingPharmacy.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // Assign the pharmacy user to the Pharmacy role
            await _userManager.AddToRoleAsync(pharmacy, "Pharmacy");

            // Remove the pending pharmacy registration from the cache
            _memoryCache.Remove(cacheKey);

            return Ok("Pharmacy approved, created, and assigned to role.");
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

        private Pharmacy MapToPharmacy(RegisterPharmacyActionRequest request,string uploadedFileName ,string logoFileName)
        {
            return new Pharmacy
            {
                UserName = request.PharmacyName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Country = request.Country,
                city = request.city,
                AccountStat = request.AccountStat,
                FormalPapersURL = uploadedFileName,
                LogoURL = logoFileName,
                CreditCardNumber = request.CreditCardNumber,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime

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


