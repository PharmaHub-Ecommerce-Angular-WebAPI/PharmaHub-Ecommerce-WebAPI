using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Infrastructure;

public interface IProductRepository : IGenericRepository<Product>
{
Task<IReadOnlyList<Product>> GetLatestProductsAsync(int page, int size);
Task<int> UpdateProductNameAsync(Guid id, string newName);
Task<int> DeleteProductAsync(Guid id);
Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(ProductCategory category);
Task<IReadOnlyList<Product>> GetProductsByNameAsync(string name);
Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
}
