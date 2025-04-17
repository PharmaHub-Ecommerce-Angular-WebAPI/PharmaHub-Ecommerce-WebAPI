using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Presentation.ActionRequest.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserActionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = MapToCustomer(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return Ok("Account Registered");

            var errors = result.Errors.Select(e => e.Description).ToArray();
            return BadRequest(errors);
        }

        // 🔁 Extracted mapping method
    
        [HttpPost]
        public async Task<IActionResult> RegisterPharmacy([FromBody] RegisterPharmacyActionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pharmacy = MapToPharmacy(request);

            var result = await _userManager.CreateAsync(pharmacy, request.Password);

            if (result.Succeeded)
            {
                return Ok("Pharmacy account registered successfully. Pending admin approval.");
            }


            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(errors);
        }
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
                    // Create JWT Token

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var roles = await _userManager.GetRolesAsync(user);

                    foreach (string role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var secret = _config["Jwt:Secret"];
                    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                    SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    // Signing Credentials = Secret + Algorithm

                    // Create JWT Token

                    JwtSecurityToken jwtToken = new JwtSecurityToken
                    (
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddHours(3),
                        signingCredentials: signingCredentials
                    );

                    return Ok
                    (
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            expiration = jwtToken.ValidTo
                        }
                    );
                }
            }

            // User doesnot exist
            return Unauthorized();
        }

        // 🔁 Mapping method
        private Pharmacy MapToPharmacy(RegisterPharmacyActionRequest request)
        {
            return new Pharmacy
            {
                UserName = request.PharmacyName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Country = request.Country,
                city = request.city,
                AccountStat = request.AccountStat,
                FormalPapersURL = request.FormalPapersURL,
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
                city = request.City
            };
        }
    }

}


