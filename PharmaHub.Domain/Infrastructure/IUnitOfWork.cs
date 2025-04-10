using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.SuggestedMedicin;

namespace PharmaHub.DAL.Repositories.UnitOfWork;

public interface IUnitOfWork 
{
    ISuggestedMedicineRepository SuggestedMedicineRepository { get; }
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> CompleteAsync();
}
