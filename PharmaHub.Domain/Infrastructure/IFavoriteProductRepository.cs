using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;

namespace PharmaHub.Domain.Infrastructure;
public interface IFavoriteProductRepository : IGenericRepository<FavoriteProduct>
{
    Task<List<FavoriteProduct>> GetFavoritesByUserIdAsync(string userId);
    Task<FavoriteProduct?> GetFavoriteByUserAndProductAsync(string userId, Guid productId);
    Task<bool> IsProductFavoritedAsync(string userId, Guid productId);
    Task<bool> RemoveFavoriteAsync(string userId, Guid productId);
}
