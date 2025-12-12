using Microsoft.EntityFrameworkCore.Storage;
using MarketingAgent.Core.Interfaces;
using MarketingAgent.Infrastructure.Data;

namespace MarketingAgent.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing database transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly MarketingAgentDbContext _context;
    private IDbContextTransaction? _transaction;
    
    private ILeadRepository? _leadRepository;
    private ILeadScoreRepository? _leadScoreRepository;
    private ILeadSourceAttributionRepository? _leadSourceAttributionRepository;
    
    public UnitOfWork(MarketingAgentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public ILeadRepository Leads
    {
        get
        {
            _leadRepository ??= new LeadRepository(_context);
            return _leadRepository;
        }
    }
    
    public ILeadScoreRepository LeadScores
    {
        get
        {
            _leadScoreRepository ??= new LeadScoreRepository(_context);
            return _leadScoreRepository;
        }
    }
    
    public ILeadSourceAttributionRepository LeadSourceAttributions
    {
        get
        {
            _leadSourceAttributionRepository ??= new LeadSourceAttributionRepository(_context);
            return _leadSourceAttributionRepository;
        }
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }
        
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
    
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }
        
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
