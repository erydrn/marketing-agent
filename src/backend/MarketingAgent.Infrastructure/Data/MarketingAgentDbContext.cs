using Microsoft.EntityFrameworkCore;
using MarketingAgent.Core.Entities;

namespace MarketingAgent.Infrastructure.Data;

/// <summary>
/// Database context for the Marketing Agent application
/// </summary>
public class MarketingAgentDbContext : DbContext
{
    public MarketingAgentDbContext(DbContextOptions<MarketingAgentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<LeadScore> LeadScores => Set<LeadScore>();
    public DbSet<LeadSourceAttribution> LeadSourceAttributions => Set<LeadSourceAttribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Lead entity configuration
        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.ExternalLeadId).IsUnique();
            entity.HasIndex(e => e.Postcode);
            entity.HasIndex(e => e.CreatedAt);
            
            entity.HasQueryFilter(e => e.DeletedAt == null); // Soft delete filter

            entity.HasOne(e => e.LeadScore)
                .WithOne(s => s.Lead)
                .HasForeignKey<LeadScore>(s => s.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.SourceAttributions)
                .WithOne(a => a.Lead)
                .HasForeignKey(a => a.LeadId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // LeadScore entity configuration
        modelBuilder.Entity<LeadScore>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LeadId);
            entity.HasIndex(e => e.Tier);
        });

        // LeadSourceAttribution entity configuration
        modelBuilder.Entity<LeadSourceAttribution>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LeadId);
            entity.HasIndex(e => e.Channel);
            entity.HasIndex(e => e.Source);
        });
    }
}
