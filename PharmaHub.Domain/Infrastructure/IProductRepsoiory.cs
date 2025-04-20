using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Objects;

namespace PharmaHub.Domain.Infrastructure;

public interface IProductRepository : IGenericRepository<Product>
{

 public Task<IReadOnlyList<Product>> GetLatestProductsAsync(int page, int size, int maxPrice, bool Offer, string pharmacyId, Governorates city, params ProductCategory[] categories);
 public  Task<IReadOnlyList<Product>> GetProductsByPharmacyIdAsync(string pharmacyId);
 public Task<IReadOnlyList<Product>> GetRangeProductsByIdsAsync(List<Guid> productIds);
public Task<int> UpdateProductNameAsync(Guid id, string newName);
public Task<List<string>> GetRelatedComponents(Guid productId);
public Task<int> DeleteProductAsync(Guid id);
public Task<IReadOnlyList<Product>> GetProductsByNameAsync(string name);
public Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    public Task<PharmacyProductStats> GetPharmacyAnalisisAsync(string pharmacyId);

}
