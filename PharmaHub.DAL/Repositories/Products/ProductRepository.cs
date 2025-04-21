using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Infrastructure;
using PharmaHub.Domain.Objects;

namespace PharmaHub.DAL.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }

    /*
     * 1 - Get all the data from specific pharmacy Or random.
     * 2 - Get the product that have offer or have not .
     * 3 - Get the Product by the Products in the city
     * 
     */
    public async Task<IReadOnlyList<Product>> GetLatestProductsAsync(
    int page,
    int sizePerCategory,
    int maxPrice,
    bool offer,
    string pharmacyId,
    Governorates city,
    params ProductCategory[] categories)
    {
        var result = new List<Product>();
        if (categories == null || categories.Length == 0)
            return new List<Product>();

        foreach (var category in categories)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.Pharmacy)
                .Include(p => p.PackagesComponents)
                .Where(p => p.Category == category && p.Quantity > 0);
            


            if (maxPrice > 0)
                query = query.Where(p => p.Price <= maxPrice);

            if (!string.IsNullOrEmpty(pharmacyId))
                query = query.Where(p => p.PharmacyId == pharmacyId);

            query = offer
                ? query.Where(p => p.DiscountRate > 0)
                : query.Where(p => p.DiscountRate == 0);


             query = query.Where(p => p.Pharmacy.city == city); 


            var categoryProducts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * sizePerCategory)
                .Take(sizePerCategory)
                .ToListAsync();

            // Add the products of the current category to the result list

            result.AddRange(categoryProducts);
        }
        return result;
    }

    public async Task<IReadOnlyList<Product>> GetProductsByPharmacyIdAsync(string pharmacyId)
    {
        var products = await _dbSet
            .AsNoTracking()
            .Where(p => p.PharmacyId == pharmacyId)
            .ToListAsync();
        return products;

    }

    // Get the Maximum Price of the products in each category

    public async Task<decimal> GetMaxPriceByCategoryAsync(ProductCategory? category)
    {
       var query =  _dbSet
                        .AsNoTracking();
        if (category != null)
            return await query.Where(p => p.Category == category)
                .MaxAsync(p => p.Price);

        // If category is null, return the maximum price of all products
        return await query
            .MaxAsync(p => p.Price);
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
            .Include(p => p.Pharmacy)
            .Include(p => p.PackagesComponents)
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

    public async Task<PharmacyProductStats> GetPharmacyAnalisisAsync(string pharmacyId)
    {

        // Total number of products in this pharmacy
        var totalProductsCount = await _dbSet
            .AsNoTracking()
            .CountAsync(p => p.PharmacyId == pharmacyId);

        // Number of products that have been favorited by any customer
        var favoritedProductsCount = await _dbSet
            .AsNoTracking()
            .Include(f=>f.FavoriteProductsList)
            .Where(p => p.PharmacyId == pharmacyId && p.FavoriteProductsList.Any())
            .CountAsync();

        // Return the counts as a list
        return new PharmacyProductStats { TotalProducts =totalProductsCount, FavoritedProducts = favoritedProductsCount };
    }
}