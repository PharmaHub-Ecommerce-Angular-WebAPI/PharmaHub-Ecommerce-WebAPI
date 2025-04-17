using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;
using PharmaHub.DTOs.SuggestedMedicineDTOs;
using PharmaHub.Service.OpenFDA_Handler;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestedMedicinesController : ControllerBase
    {
        private readonly ISuggestedMedicineManager _suggestedManager;
        private readonly OpenFDAService _openFdaService;

        public SuggestedMedicinesController(ISuggestedMedicineManager suggestedManager, OpenFDAService openFdaService)
        {
            _suggestedManager = suggestedManager;
            _openFdaService = openFdaService;
        }



        //// POST: api/suggestedmedicines
        //[HttpPost]
        //public async Task<IActionResult> AddSuggestedMedicine([FromBody] CreateSuggestedMedicineDto newMed)
        //{
        //    await _suggestedManager.AddSuggestedMedicineAsync(newMed);
        //    return StatusCode(201); // Created
        //}

        //// DELETE: api/suggestedmedicines/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSuggestedMedicine(Guid id)
        //{
        //    await _suggestedManager.RemoveSuggestedMedicineAsync(id);
        //    return NoContent();
        //}

       
        
        
        [HttpGet("search")]
        // GET: api/suggestedmedicines/search?name=paracetamol
        public async Task<IActionResult> SearchSuggestedMedicine([FromQuery] string name)
        {
            var results = await _suggestedManager.SearchSuggestedMedicineAsync(name);
            if (results == null || !results.Any())
            {
               var drugSuggestions=await  _openFdaService.SearchDrugOpenFDA(name);
                if (drugSuggestions == null || !drugSuggestions.Any())
                {
                    return NotFound("No suggested medicines found.");
                }
                foreach (var drug in drugSuggestions)
                {
                    var newMed = new GetSuggestedMedicineDto
                    {
                        Id = Guid.NewGuid(), 
                        Name = drug.Name,
                        Strength= (drug.Strength >= short.MinValue && drug.Strength <= short.MaxValue)
                                                    ? Convert.ToInt16(drug.Strength)
                                                    : (short)0
                    };
                    await _suggestedManager.AddSuggestedMedicineAsync(newMed);//make add range for it to add the suggested medicines
                }
            }
            return Ok(results);
        }



    }
}
