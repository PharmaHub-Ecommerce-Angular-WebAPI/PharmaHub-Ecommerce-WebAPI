using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;

namespace PharmaHub.DAL.Repositories.SuggestedMedicin;

public interface ISuggestedMedicineRepository 
{
    Task<IEnumerable<SuggestedMedicine>> GetAllByUserAsync(Guid userId);
    Task AddSuggestedMedicineAsync(SuggestedMedicine newMed);
}