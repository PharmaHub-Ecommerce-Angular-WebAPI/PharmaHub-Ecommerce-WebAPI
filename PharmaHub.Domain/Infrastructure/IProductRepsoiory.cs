using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Infrastructure;

public interface IProductRepository : IGenericRepository<Product>
{

 public Task<IReadOnlyList<Product>> GetLatestProductsAsync(int page, int size, int maxPrice, bool Offer, string pharmacyId, params ProductCategory[] categories);
    public Task<IReadOnlyList<Product>> GetRangeProductsByIdsAsync(List<Guid> productIds);
public Task<int> UpdateProductNameAsync(Guid id, string newName);
public Task<List<string>> GetRelatedComponents(Guid productId);
public Task<int> DeleteProductAsync(Guid id);
public Task<IReadOnlyList<Product>> GetProductsByNameAsync(string name);
public Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

}
