using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private Dictionary<Type, object> _repositories = new();

    // Implement all interface members
    public ISuggestedMedicineRepository _suggestedMedicinesRepo { get; }
    public IProductRepository _productsRepo { get; }
    public IOrderRepository _ordersRepo { get; }
    public IFavoriteProductRepository _favoriteProductsRepo { get; }
    public IPackagesComponentRepository _PackagesComponentRepo { get; }

    public UnitOfWork(
          ApplicationDbContext context,
          ISuggestedMedicineRepository suggestedMedicinesRepo,
          IProductRepository productsRepo,
          IOrderRepository ordersRepo,
          IFavoriteProductRepository favoriteProductsRepo,
          IPackagesComponentRepository packagesComponentRepo)
    {
        _context = context;
        _suggestedMedicinesRepo = suggestedMedicinesRepo;
        _productsRepo = productsRepo;
        _ordersRepo = ordersRepo;
        _favoriteProductsRepo = favoriteProductsRepo;
        _PackagesComponentRepo = packagesComponentRepo;
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
