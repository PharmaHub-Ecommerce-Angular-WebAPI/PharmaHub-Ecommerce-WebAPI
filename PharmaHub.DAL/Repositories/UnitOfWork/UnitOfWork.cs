using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.DALRepositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private Dictionary<Type, object> _repositories =  new();

    // Implement all interface members
    public ISuggestedMedicineRepository SuggestedMedicines { get; }
    public IProductRepository Products { get; } 
    public IOrderRepository Orders { get; }
    public IFavoriteProductRepository FavoriteProducts { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        SuggestedMedicines = new SuggestedMedicineRepository(_context);
        Products = new ProductRepository(_context);
        Orders = new OrderRepository(_context);
        FavoriteProducts = new FavoriteProductRepository(_context);

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
