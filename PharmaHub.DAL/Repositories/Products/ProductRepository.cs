using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }

    // View products paginated and ordered by latest
    public async Task<IReadOnlyList<Product>> GetLatestProductsAsync(int page, int size)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt) 
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    // Update without loading using ExecuteUpdateAsync
    public async Task<int> UpdateProductNameAsync(Guid id, string newName)
    {
        return await _dbSet
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.Name, x => newName));
    }

    // Delete without loading using ExecuteDeleteAsync
    public async Task<int> DeleteProductAsync(Guid id)
    {
        return await _dbSet
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

    // Get by category 
    public async Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(ProductCategory category)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Category == category)
            .ToListAsync();
    }

    // Get by name (exact or partial)
    public async Task<IReadOnlyList<Product>> GetProductsByNameAsync(string name)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
            .ToListAsync();
    }


    // Get by price range
    public async Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToListAsync();
    }


    //Get Number of Products pages
    public async Task<int> GetNumberOfProductsPagesAsync(int pageSize)
    {
        var totalProducts = await _dbSet.CountAsync();
        return (int)Math.Ceiling((double)totalProducts / pageSize);
    }
}