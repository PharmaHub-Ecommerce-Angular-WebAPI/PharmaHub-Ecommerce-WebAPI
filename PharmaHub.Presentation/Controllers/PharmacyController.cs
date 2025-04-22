using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {

        UserManager<User> _userManager;
        
        public PharmacyController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetPharmacyById/{id}")]
        public async Task<IActionResult> GetPharmacyById(string id)
        {
            var pharmacy = await _userManager.FindByIdAsync(id);
            if (pharmacy == null)
            {
                return NotFound("Pharmacy not found.");
            }
            return Ok(pharmacy);
        }
        [HttpGet("Getpharmacies")]
        public async Task<IActionResult> GetAllPharmacyUsers(int? numberOfUsers)
        {
            if (numberOfUsers == null || numberOfUsers == 0 )
            {
                var pharmacies = await _userManager.Users
                                    .OfType<Pharmacy>()
                                    .ToListAsync();

               return Ok(pharmacies);
            }
            else
            {
                var pharmacies = await _userManager.Users.Take(numberOfUsers.Value)
                    .OfType<Pharmacy>()
                    .ToListAsync();

                return Ok(pharmacies);
            }

        }


    }
}
