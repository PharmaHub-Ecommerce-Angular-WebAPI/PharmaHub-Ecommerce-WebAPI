using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepository;
namespace PharmaHub.DAL.Repositories.GenericRepositoryl;

public class GenericRepository<T> : IGenericRepository<T>
       where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet ;

    public GenericRepository(ApplicationDbContext context )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SavaChange();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
            await SavaChange();
            return true;
        }
        return false;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    => await _dbSet.ToListAsync();


    public async Task<T?> GetIdAsync(Guid id)
    => await _dbSet.FindAsync(id);


    public async Task SavaChange()
    => await _context.SaveChangesAsync();

    public async Task<T> UpdatedAsync(T entity)
    {
        _dbSet.Update(entity);
        await SavaChange();
        return entity;
    }

    public async Task UpsertAsync(T entity)
    {
        var entry = _context.Entry(entity);

        // Check if the primary key has value and entity exists
        var keyProperty = _context.Model
            .FindEntityType(typeof(T))?
            .FindPrimaryKey()?
            .Properties
            .First();

        if (keyProperty is null)
        {
            throw new InvalidOperationException($"Entity type {typeof(T).Name} has no primary key defined.");
        }

        var keyValue = entry.Property(keyProperty.Name).CurrentValue;

        if (keyValue == null || keyValue.Equals(GetDefault(keyProperty.ClrType)))
        {
            await _dbSet.AddAsync(entity); // Insert
            
        }
        else
        {
            var existingEntity = await _dbSet.FindAsync(keyValue);
            if (existingEntity == null)
            {
                await _dbSet.AddAsync(entity); // Insert
               
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity); // Update
                
            }
        }
    }

    private static object GetDefault(Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        return type.IsValueType ? Activator.CreateInstance(type) : null!;
    }
}