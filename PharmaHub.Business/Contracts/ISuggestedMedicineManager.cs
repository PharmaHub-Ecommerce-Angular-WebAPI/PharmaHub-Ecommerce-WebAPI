using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.DTOs.SuggestedMedicineDTOs;

namespace PharmaHub.Business.Contracts
{
    public interface ISuggestedMedicineManager
    {
        public Task AddSuggestedMedicineAsync(CreateSuggestedMedicineDto newMed);
        public Task RemoveSuggestedMedicineAsync(Guid id);
        public Task<IReadOnlyList<GetSuggestedMedicineDto>> SearchSuggestedMedicineAsync(string name);

    }
}
