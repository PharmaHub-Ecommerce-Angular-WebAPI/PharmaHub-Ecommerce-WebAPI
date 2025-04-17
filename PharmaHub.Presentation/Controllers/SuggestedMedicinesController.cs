using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;
using PharmaHub.DTOs.SuggestedMedicineDTOs;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestedMedicinesController : ControllerBase
    {
        private readonly ISuggestedMedicineManager _suggestedManager;

        public SuggestedMedicinesController(ISuggestedMedicineManager suggestedManager)
        {
            _suggestedManager = suggestedManager;
        }

        // POST: api/suggestedmedicines
        [HttpPost]
        public async Task<IActionResult> AddSuggestedMedicine([FromBody] CreateSuggestedMedicineDto newMed)
        {
            await _suggestedManager.AddSuggestedMedicineAsync(newMed);
            return StatusCode(201); // Created
        }

        // DELETE: api/suggestedmedicines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuggestedMedicine(Guid id)
        {
            await _suggestedManager.RemoveSuggestedMedicineAsync(id);
            return NoContent();
        }

        // GET: api/suggestedmedicines/search?name=paracetamol
        [HttpGet("search")]
        public async Task<IActionResult> SearchSuggestedMedicine([FromQuery] string name)
        {
            var results = await _suggestedManager.SearchSuggestedMedicineAsync(name);
            if (results == null || !results.Any())
            {
                return NotFound("Medicine Not Found");
            }
            return Ok(results);
        }



    }
}
