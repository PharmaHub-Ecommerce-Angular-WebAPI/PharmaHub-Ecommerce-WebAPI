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
        public async Task<IActionResult> SearchSuggestedMedicine([FromQuery] string name)
        {
            // Validate the input
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be null or empty.");
            }
            // Search for suggested medicines in my database
            var results = await _suggestedManager.SearchSuggestedMedicineAsync(name);
            // If no results found, search in OpenFDA
            if (results == null || !results.Any())
            {
                // Search for suggested medicines in OpenFDA
                var drugSuggestions = await _openFdaService.SearchDrugOpenFDA(name);
                // If no suggestions found, return NotFound
                if (drugSuggestions == null || !drugSuggestions.Any())
                {
                    return NotFound("No suggested medicines found.");
                }

                // Map the results to the DTO
                var CreateSuggestedMedicineDto = drugSuggestions.Select(d => new CreateSuggestedMedicineDto
                {
                    Id = Guid.NewGuid(), // Generate a new GUID for the ID
                    Name = d.Name,
                    Strength = (d.Strength >= short.MinValue && d.Strength <= short.MaxValue)
                                                   ? Convert.ToInt16(d.Strength)
                                                   : (short)0
                }).ToArray();

                // Add it in my database
                await _suggestedManager.AddRangeSuggestedMedicineAsync(CreateSuggestedMedicineDto);

                // After adding the new medicines in my database, Send the results
                results = CreateSuggestedMedicineDto.Select(C=> 
                    new GetSuggestedMedicineDto
                    {
                        Id = C.Id,
                        Name = C.Name,
                        Strength = C.Strength
                    } 
                ).ToList();

            }
            return Ok(results);
        }
    }
}