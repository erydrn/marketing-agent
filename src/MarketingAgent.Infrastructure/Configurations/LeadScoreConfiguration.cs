using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketingAgent.Core.Entities;

namespace MarketingAgent.Infrastructure.Configurations;

/// <summary>
/// Entity configuration for LeadScore entity
/// </summary>
public class LeadScoreConfiguration : IEntityTypeConfiguration<LeadScore>
{
    public void Configure(EntityTypeBuilder<LeadScore> builder)
    {
        builder.ToTable("LeadScores");
        
        // Primary Key
        builder.HasKey(ls => ls.Id);
        
        // Indexes
        builder.HasIndex(ls => ls.LeadId)
            .IsUnique()
            .HasDatabaseName("IX_LeadScores_LeadId");
        
        builder.HasIndex(ls => ls.Tier)
            .HasDatabaseName("IX_LeadScores_Tier");
        
        builder.HasIndex(ls => ls.OverallScore)
            .HasDatabaseName("IX_LeadScores_OverallScore");
        
        // Properties
        builder.Property(ls => ls.LeadId)
            .IsRequired();
        
        builder.Property(ls => ls.OverallScore)
            .IsRequired();
        
        builder.Property(ls => ls.Tier)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
        
        builder.Property(ls => ls.CompletenessScore)
            .IsRequired();
        
        builder.Property(ls => ls.EngagementScore)
            .IsRequired();
        
        builder.Property(ls => ls.ReadinessScore)
            .IsRequired();
        
        builder.Property(ls => ls.SourceQualityScore)
            .IsRequired();
        
        builder.Property(ls => ls.CalculatedAt)
            .IsRequired();
        
        // Concurrency token
        builder.Property(ls => ls.Version)
            .IsConcurrencyToken();
        
        // Ignore calculated properties
        builder.Ignore(ls => ls.IsDeleted);
    }
}

/// <summary>
/// Entity configuration for LeadSourceAttribution entity
/// </summary>
public class LeadSourceAttributionConfiguration : IEntityTypeConfiguration<LeadSourceAttribution>
{
    public void Configure(EntityTypeBuilder<LeadSourceAttribution> builder)
    {
        builder.ToTable("LeadSourceAttributions");
        
        // Primary Key
        builder.HasKey(lsa => lsa.Id);
        
        // Indexes
        builder.HasIndex(lsa => lsa.LeadId)
            .IsUnique()
            .HasDatabaseName("IX_LeadSourceAttributions_LeadId");
        
        builder.HasIndex(lsa => lsa.Channel)
            .HasDatabaseName("IX_LeadSourceAttributions_Channel");
        
        builder.HasIndex(lsa => lsa.Source)
            .HasDatabaseName("IX_LeadSourceAttributions_Source");
        
        builder.HasIndex(lsa => lsa.Campaign)
            .HasDatabaseName("IX_LeadSourceAttributions_Campaign");
        
        // Properties
        builder.Property(lsa => lsa.LeadId)
            .IsRequired();
        
        builder.Property(lsa => lsa.Channel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(lsa => lsa.Source)
            .HasMaxLength(100);
        
        builder.Property(lsa => lsa.Campaign)
            .HasMaxLength(200);
        
        builder.Property(lsa => lsa.Medium)
            .HasMaxLength(50);
        
        builder.Property(lsa => lsa.UtmSource)
            .HasMaxLength(100);
        
        builder.Property(lsa => lsa.UtmMedium)
            .HasMaxLength(100);
        
        builder.Property(lsa => lsa.UtmCampaign)
            .HasMaxLength(200);
        
        builder.Property(lsa => lsa.UtmContent)
            .HasMaxLength(200);
        
        builder.Property(lsa => lsa.UtmTerm)
            .HasMaxLength(100);
        
        builder.Property(lsa => lsa.Referrer)
            .HasMaxLength(500);
        
        builder.Property(lsa => lsa.LandingPage)
            .HasMaxLength(500);
        
        builder.Property(lsa => lsa.CapturedAt)
            .IsRequired();
        
        // Concurrency token
        builder.Property(lsa => lsa.Version)
            .IsConcurrencyToken();
        
        // Ignore calculated properties
        builder.Ignore(lsa => lsa.IsDeleted);
    }
}
