using Microsoft.EntityFrameworkCore;
using MarketingAgent.Core.Entities;
using MarketingAgent.Core.Interfaces;
using MarketingAgent.Infrastructure.Data;

namespace MarketingAgent.Infrastructure.Repositories;

/// <summary>
/// Base repository implementation providing common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly MarketingAgentDbContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public BaseRepository(MarketingAgentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }
    
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Max page size limit
        
        var query = _dbSet.AsNoTracking();
        
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (items, totalCount);
    }
    
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));
        
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }
    
    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));
        
        entity.UpdatedAt = DateTime.UtcNow;
        entity.Version++;
        
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }
    
    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
    
    public virtual async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null && !entity.IsDeleted)
        {
            entity.DeletedAt = DateTime.UtcNow;
            await UpdateAsync(entity, cancellationToken);
        }
    }
    
    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }
    
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }
}
