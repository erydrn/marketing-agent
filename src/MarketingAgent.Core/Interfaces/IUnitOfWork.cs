namespace MarketingAgent.Core.Interfaces;

/// <summary>
/// Unit of Work pattern for managing transactions across multiple repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Lead repository instance
    /// </summary>
    ILeadRepository Leads { get; }
    
    /// <summary>
    /// Lead score repository instance
    /// </summary>
    ILeadScoreRepository LeadScores { get; }
    
    /// <summary>
    /// Lead source attribution repository instance
    /// </summary>
    ILeadSourceAttributionRepository LeadSourceAttributions { get; }
    
    /// <summary>
    /// Save all pending changes in a single transaction
    /// </summary>
    /// <returns>Number of entities affected</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begin a new database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Commit the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Rollback the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
