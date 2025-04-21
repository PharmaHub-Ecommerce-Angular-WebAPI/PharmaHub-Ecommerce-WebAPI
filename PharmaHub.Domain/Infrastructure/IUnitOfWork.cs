using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories;

public interface IUnitOfWork 
{
    ISuggestedMedicineRepository _suggestedMedicinesRepo { get; }
    IProductRepository _productsRepo { get; }
    IOrderRepository _ordersRepo { get; }
    IFavoriteProductRepository _favoriteProductsRepo { get; }
    IPackagesComponentRepository _PackagesComponentRepo { get; }

    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> CompleteAsync();
}
