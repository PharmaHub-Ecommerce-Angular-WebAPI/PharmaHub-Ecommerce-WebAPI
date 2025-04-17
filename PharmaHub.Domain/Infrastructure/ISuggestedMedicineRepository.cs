using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;

namespace PharmaHub.DAL.Repositories.SuggestedMedicin;

public interface ISuggestedMedicineRepository : IGenericRepository<SuggestedMedicine>
{
    
  public  Task AddRangeSuggestedMedicineAsync(params SuggestedMedicine[] newMed);
  public Task<IReadOnlyList<SuggestedMedicine>> GetSuggestedMedicineByName(string name);

}