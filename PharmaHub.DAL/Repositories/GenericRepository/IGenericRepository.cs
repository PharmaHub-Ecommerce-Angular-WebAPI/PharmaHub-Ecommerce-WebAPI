namespace PharmaHub.DAL.Repositories.GenericRepository;

public interface IGenericRepository <T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<T?> GetIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> UpdatedAsync(T entity);
    Task<bool> UpsertAsync(T entity);
    Task SavaChange();
}
