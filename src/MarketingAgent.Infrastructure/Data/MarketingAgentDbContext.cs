using Microsoft.EntityFrameworkCore;
using MarketingAgent.Core.Entities;

namespace MarketingAgent.Infrastructure.Data;

/// <summary>
/// Main database context for the Marketing Agent application
/// </summary>
public class MarketingAgentDbContext : DbContext
{
    public MarketingAgentDbContext(DbContextOptions<MarketingAgentDbContext> options)
        : base(options)
    {
    }
    
    /// <summary>
    /// Leads dataset
    /// </summary>
    public DbSet<Lead> Leads => Set<Lead>();
    
    /// <summary>
    /// Lead scores dataset
    /// </summary>
    public DbSet<LeadScore> LeadScores => Set<LeadScore>();
    
    /// <summary>
    /// Lead source attributions dataset
    /// </summary>
    public DbSet<LeadSourceAttribution> LeadSourceAttributions => Set<LeadSourceAttribution>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarketingAgentDbContext).Assembly);
        
        // Global query filter to exclude soft-deleted entities
        modelBuilder.Entity<Lead>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<LeadScore>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<LeadSourceAttribution>().HasQueryFilter(e => e.DeletedAt == null);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps before saving
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));
        
        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;
            
            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
                entity.Version++;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}
