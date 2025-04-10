using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.DALRepositories;

namespace PharmaHub.DAL.Repositories.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private Dictionary<Type, object> _repositories =  new();

    public ISuggestedMedicineRepository SuggestedMedicineRepository => new SuggestedMedicineRepository(_context);
    // other repositories...

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (_repositories.TryGetValue(type, out var instance))
        {
            return (IGenericRepository<T>)instance;
        }

        var repositoryType = typeof(GenericRepository<>);
        var repositoryInstance = Activator.CreateInstance(
            repositoryType.MakeGenericType(type), _context) as IGenericRepository<T>;

        _repositories.TryAdd(type, repositoryInstance);
        return repositoryInstance;
    }

    public async Task<int> CompleteAsync()
    => await _context.SaveChangesAsync();

}
