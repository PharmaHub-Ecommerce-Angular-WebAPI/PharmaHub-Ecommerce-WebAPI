using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.Presentation.Model;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/admin")] 
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        
        private ILogger<AdminController> _logger;
        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Add your admin endpoints here
        [HttpGet("secure-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetSecureData()
        {
            return Ok("This is admin-only data");
        }

        #region Pharmacy Controllers
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
        [HttpGet("pharmacies/with-status")]
        public async Task<IActionResult> GetAllUsersWithStatus()
        {
            try
            {
                var users = await _userManager.Users
                    .Where(u => u.AccountStat == AccountStats.Pending)
                    .ToListAsync();

                var result = new List<UserWithStatus>();

                foreach (var user in users)
                {
                    var pharmacy = user as Pharmacy;
                    if (pharmacy != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        result.Add(new UserWithStatus
                        {
                            Id = user.Id,
                            Email = user.Email,
                            UserName = user.UserName,
                            FormalPapersURL = pharmacy.FormalPapersURL
                        });
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users with status");
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }

        #endregion
    }
}
