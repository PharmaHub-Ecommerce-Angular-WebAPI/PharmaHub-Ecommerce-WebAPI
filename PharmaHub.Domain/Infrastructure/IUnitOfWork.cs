using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories;

public interface IUnitOfWork 
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> CompleteAsync();
}
