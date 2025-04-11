using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;

namespace PharmaHub.Domain.Infrastructure;
public interface IFavoriteProductRepository : IGenericRepository<FavoriteProduct>
{
    Task<List<FavoriteProduct>> GetFavoritesByUserIdAsync(Guid userId);
    Task<FavoriteProduct?> GetFavoriteByUserAndProductAsync(Guid userId, Guid productId);
    Task<bool> IsProductFavoritedAsync(Guid userId, Guid productId);
    Task<bool> RemoveFavoriteAsync(Guid userId, Guid productId);
}
