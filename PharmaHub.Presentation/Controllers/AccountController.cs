using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.Presentation.ActionRequest.Account;
using PharmaHub.Presentation.Extensions;
using PharmaHub.Service.JWT_Handler;
using PharmaHub.Service.PhotoHandler;
using PharmaHub.Service.UserHandler;
using PharmaHub.Service.UserHandler.Verification;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region field and Prop
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtService;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IEmailService _emailService;
        private CloudinaryService _cloudinaryService;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<User> userManager,
            JwtTokenService jwtTokenService,
            IVerificationCodeService verificationCodeService,
            IEmailService emailService,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _jwtService=jwtTokenService;
            _verificationCodeService=verificationCodeService;
            _emailService=emailService;
            _configuration = configuration;
        }

        #endregion

        #region Shared Verification Code
        #region send-verification-code
        [HttpPost("send-verification-code")]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendCodeRequest request)
        {
            var email = request.Email?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            if (_verificationCodeService.HasTooManyAttempts(email))
                return BadRequest("Too many attempts. Please try again later.");

            var code = _verificationCodeService.GenerateAndStoreCode(email);
            await _emailService.SendVerificationCode(email, code, request.Name);

            return Ok(new { Message = "Verification code sent to your email." });
        }
        #endregion

        #region verify-code
        [HttpPost("verify-code")]
        public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
        {
            var email = request.Email?.Trim().ToLower();
            var result = _verificationCodeService.VerifyCode(email, request.VerificationCode);

            if (result == "Invalid")
                return BadRequest($"Invalid verification code. You have {_verificationCodeService.GetRemainingAttempts(email)} attempts remaining.");

            if (result == "Expired")
                return BadRequest("Verification code has expired. Please request a new one.");

            return Ok("Verification successful.");
        }


        #endregion

        #region resend-verification
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

        #endregion

        #region Register with Built-in Email Verification
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserActionRequest request)
        {
            var email = request.Email.ToLower();

            var isVerified = _verificationCodeService.IsVerified(email);
            Console.WriteLine($"IsVerified check for {email}: {isVerified}");
            if (!isVerified)
            {
                return BadRequest("Email is not verified.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("Email is already registered");

            var user = MapToCustomer(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _userManager.AddToRoleAsync(user, "Customer");
                _verificationCodeService.ClearCode(request.Email);

                return Ok("Account registered successfully");
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        #endregion

        #region RegisterPharmacy
        [HttpPost("pharmacyregister")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterPharmacy([FromForm] RegisterPharmacyActionRequest request)
        {
            // Normalize email before validation
            request.Email = request.Email?.Trim().ToLower();

            var isVerified = _verificationCodeService.IsVerified(request.Email);
            Console.WriteLine($"IsVerified check for {request.Email}: {isVerified}");
            if (!isVerified)
            {
                return BadRequest("Email is not verified.");
            }

            // Validate the file type
            if (request.FormalPapersURL is null)
            {
                return BadRequest(new { message = "FormalPapersURL was not received." });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate FormalPapersURL
            if (request.FormalPapersURL == null || request.FormalPapersURL.Length == 0)
                return BadRequest(new { message = "FormalPapersURL was not received." });

            // Validate file size (limit to 5MB)
            if (request.FormalPapersURL.Length > 5 * 1024 * 1024)
                return BadRequest("Formal paper file size exceeds the limit of 5MB.");

            // Validate file type (only allow PDF)
            var allowedPdfExtensions = new[] { ".pdf" };
            var formalPapersExtension = Path.GetExtension(request.FormalPapersURL.FileName).ToLower();
            if (!allowedPdfExtensions.Contains(formalPapersExtension))
                return BadRequest("Invalid formal paper format. Only PDF is allowed.");

            // Validate the image file
            var cloudName = _configuration["Cloudinary:CloudName"];
            var apiKey = _configuration["Cloudinary:ApiKey"];
            var apiSecret = _configuration["Cloudinary:ApiSecret"];

            // Check if Cloudinary configuration is set
            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                return BadRequest("Cloudinary configuration is missing.");
            }
            _cloudinaryService = new CloudinaryService(cloudName, apiKey, apiSecret);


            // Validate LogoURL if provided
            string logoFileName = string.Empty;
            if (!(request.LogoURL == null || request.LogoURL.Length == 0))
            {
                // Validate the image file size (5MB limit)
                if (request.LogoURL.Length > 5 * 1024 * 1024)
                {
                    return BadRequest("Image size exceeds the limit of 5MB.");
                }
                // Validate the image file type (only allow jpg, jpeg, png)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(request.LogoURL.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid image format. Only jpg, jpeg, and png are allowed.");
                }

                // Check if Cloudinary configuration is set
                if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                {
                    return BadRequest("Cloudinary configuration is missing.");
                }
                _cloudinaryService = new CloudinaryService(cloudName, apiKey, apiSecret);

                // Upload the image to Cloudinary
                logoFileName  =  await _cloudinaryService.UploadImageAsync(request.LogoURL);
            }

            // Check if the email is already registered
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("Email is already registered");

          
                var uploadedFileName = await _cloudinaryService.UploadImageAsync(request.FormalPapersURL);
                if (request.LogoURL != null)
                {
                    logoFileName = await _cloudinaryService.UploadImageAsync(request.LogoURL);
                }

                var pharmacy = MapToPharmacy(request, uploadedFileName, logoFileName);
                var result = await _userManager.CreateAsync(pharmacy, request.Password);

                if (result.Succeeded)
                {
                    pharmacy.EmailConfirmed = true;
                    await _userManager.AddToRoleAsync(pharmacy, "Pharmacy");
                    _verificationCodeService.ClearCode(request.Email);
                    return Ok("Account registered successfully");
                }

                return BadRequest(result.Errors.Select(e => e.Description));
            
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("pharmacy/approve/{email}")]
        public async Task<IActionResult> ApprovePharmacy(string email)
        {
            try
            {
                // Find the pharmacy user by email
                var pharmacy = await _userManager.FindByEmailAsync(email);
                if (pharmacy == null)
                    return NotFound("Pharmacy not found");

                // Check if already approved
                if (pharmacy.AccountStat == AccountStats.Active)
                    return Ok("Pharmacy is already approved");

                // Update status to approved
                pharmacy.AccountStat = AccountStats.Active;

                // Update the user
                var result = await _userManager.UpdateAsync(pharmacy);
                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                // Assign to Pharmacy role if needed
                if (!await _userManager.IsInRoleAsync(pharmacy, "Pharmacy"))
                {
                    var roleResult = await _userManager.AddToRoleAsync(pharmacy, "Pharmacy");
                    if (!roleResult.Succeeded)
                        return BadRequest(roleResult.Errors.Select(e => e.Description));
                }

                return Ok("Pharmacy approved successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("pharmacy/ban/{email}")]
        public async Task<IActionResult> BanPharmacy(string email)
        {
            try
            {
                // Find the pharmacy user by email
                var pharmacy = await _userManager.FindByEmailAsync(email);
                if (pharmacy == null)
                    return NotFound("Pharmacy not found");

                // Check if already banned
                if (pharmacy.AccountStat == AccountStats.Banned)
                    return Ok("Pharmacy is already banned");

                // Update status to banned
                pharmacy.AccountStat = AccountStats.Banned;

                // Update the user
                var result = await _userManager.UpdateAsync(pharmacy);
                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                // Remove from Pharmacy role (optional - depending on your requirements)
                if (await _userManager.IsInRoleAsync(pharmacy, "Pharmacy"))
                {
                    var roleResult = await _userManager.RemoveFromRoleAsync(pharmacy, "Pharmacy");
                    if (!roleResult.Succeeded)
                        return BadRequest(roleResult.Errors.Select(e => e.Description));
                }

                return Ok("Pharmacy banned successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("users/with-status")]
        public async Task<IActionResult> GetAllUsersWithStatus()
        {
            try
            {
                // Get all users (or filter by role if needed)
                List<Pharmacy>? users = _userManager.User.ToList();

                var result = new List<UserWithStatus>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(Pharmacy);
                    result.Add(new UserWithStatusDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        AccountStat = user.AccountStat,
                        Roles = roles.ToList()
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching users.");
            }
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
                PharmacyName = request.PharmacyName,
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Country = request.Country,
                city = request.city,
                AccountStat = request.AccountStat,
                FormalPapersURL = uploadedFileName,
                LogoURL = logoFileName,
                CreditCardNumber = request.CreditCardNumber,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,
                Address = request.Address
               
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