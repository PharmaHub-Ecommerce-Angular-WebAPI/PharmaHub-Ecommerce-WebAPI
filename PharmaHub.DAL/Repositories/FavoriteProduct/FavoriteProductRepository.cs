using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories;

public class FavoriteProductRepository : GenericRepository<FavoriteProduct>////////Fix Inert from lock interface 
{
    public FavoriteProductRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<FavoriteProduct>> GetFavoritesByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(fp => fp.CustomerId == userId)
            .Include(fp => fp.Product) 
            .ToListAsync();
    }

    public async Task<FavoriteProduct?> GetFavoriteByUserAndProductAsync(string userId, Guid productId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(fp => fp.CustomerId ==userId  && fp.ProductId == productId);
    }

    public async Task<bool> IsProductFavoritedAsync(string  userId, Guid productId)
    {
        return await _dbSet
            .AnyAsync(fp => fp.CustomerId == userId && fp.ProductId == productId);
    }

    public async Task<bool> RemoveFavoriteAsync(string userId, Guid productId)
    {
        var fav = await GetFavoriteByUserAndProductAsync(userId, productId);
        if (fav != null)
        {
            _dbSet.Remove(fav);
            await SavaChange();
            return true;
        }
        return false;
    }
}