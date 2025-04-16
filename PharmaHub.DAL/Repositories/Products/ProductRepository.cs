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

    // View products paginated and ordered by latest For home page
    //public async Task<IReadOnlyList<Product>> GetLatestProductsAsync(int page, int size, int maxPrice, bool Offer,string pharmacyId , params ProductCategory[] categories)
    //{
    //    var query = _dbSet
    //        .AsNoTracking()
    //        .Where(p => categories.Contains(p.Category) && p.Quantity > 0);

    //        if (maxPrice > 0)
    //        {
    //        query = query.Where(p => p.Price <= maxPrice);
    //        }
    //        if(!string.IsNullOrEmpty(pharmacyId))
    //         {
    //        query = query.Where(p => p.PharmacyId == pharmacyId);
    //    }
    //        if (Offer)
    //        {
    //        query = query.Where(p => p.DiscountRate > 0);
    //        }
    //        else
    //        {
    //        query = query.Where(p => p.DiscountRate == 0);
    //        }

    //    query = query
    //        .OrderByDescending(p => p.CreatedAt)
    //        .Skip((page - 1) * size)
    //        .Take(size);
    //    return await query.ToListAsync();
    //}

    public async Task<IReadOnlyList<Product>> GetLatestProductsAsync(
    int page,
    int sizePerCategory,
    int maxPrice,
    bool offer,
    string pharmacyId,
    params ProductCategory[] categories)
    {
        var result = new List<Product>();

        foreach (var category in categories)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(p => p.Category == category && p.Quantity > 0);

            if (maxPrice > 0)
                query = query.Where(p => p.Price <= maxPrice);

            if (!string.IsNullOrEmpty(pharmacyId))
                query = query.Where(p => p.PharmacyId == pharmacyId);

            query = offer
                ? query.Where(p => p.DiscountRate > 0)
                : query.Where(p => p.DiscountRate == 0);

            var categoryProducts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * sizePerCategory)
                .Take(sizePerCategory)
                .ToListAsync();

            result.AddRange(categoryProducts);
        }

        return result;
    }

    public async Task<IReadOnlyList<Product>> GetRangeProductsByIdsAsync(List<Guid> productIds)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id)) 
            .ToListAsync();
    }


    public async Task<List<string>> GetRelatedComponents(Guid productId)
    {
        var components = await _dbSet
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .SelectMany(p => p.PackagesComponents.Select(pc => pc.ComponentName))
            .ToListAsync();
        return components;
    }

    // Get product Offers
    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await _dbSet
            .AsNoTracking()
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