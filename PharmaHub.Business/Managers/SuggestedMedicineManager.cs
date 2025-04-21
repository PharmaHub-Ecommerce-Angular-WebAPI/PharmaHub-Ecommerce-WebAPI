using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Business.Contracts;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.DTOs.SuggestedMedicineDTOs;

namespace PharmaHub.Business.Managers
{
    public class SuggestedMedicineManager: ISuggestedMedicineManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public SuggestedMedicineManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRangeSuggestedMedicineAsync(params CreateSuggestedMedicineDto [] newMed)
        {
            if (newMed == null)
                throw new ArgumentNullException(nameof(newMed));
            await _unitOfWork._suggestedMedicinesRepo.AddRangeSuggestedMedicineAsync(
                newMed.Select(m => new SuggestedMedicine
                {
                    Id = m.Id,
                    Name = m.Name,
                    Strength = m.Strength
                }).ToArray());
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveSuggestedMedicineAsync(Guid id)
        {
            await _unitOfWork._suggestedMedicinesRepo.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IReadOnlyList<GetSuggestedMedicineDto>> SearchSuggestedMedicineAsync(string name)
        {
            var suggestProduct = await _unitOfWork._suggestedMedicinesRepo.GetSuggestedMedicineByName(name);
            return suggestProduct.Select(s => new GetSuggestedMedicineDto
            {
                Id = s.Id,
                Name = s.Name,
                Strength = s.Strength
            }) .ToList();
        }

    }
}
