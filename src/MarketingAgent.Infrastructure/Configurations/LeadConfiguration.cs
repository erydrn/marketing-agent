using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketingAgent.Core.Entities;

namespace MarketingAgent.Infrastructure.Configurations;

/// <summary>
/// Entity configuration for Lead entity
/// </summary>
public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");
        
        // Primary Key
        builder.HasKey(l => l.Id);
        
        // Indexes
        builder.HasIndex(l => l.ExternalLeadId)
            .IsUnique()
            .HasDatabaseName("IX_Leads_ExternalLeadId");
        
        builder.HasIndex(l => l.Email)
            .HasDatabaseName("IX_Leads_Email");
        
        builder.HasIndex(l => l.Postcode)
            .HasDatabaseName("IX_Leads_Postcode");
        
        builder.HasIndex(l => l.CreatedAt)
            .HasDatabaseName("IX_Leads_CreatedAt");
        
        builder.HasIndex(l => l.DeletedAt)
            .HasDatabaseName("IX_Leads_DeletedAt");
        
        // Properties
        builder.Property(l => l.ExternalLeadId)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(l => l.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(l => l.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(l => l.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(l => l.Phone)
            .HasMaxLength(50);
        
        builder.Property(l => l.Address)
            .HasMaxLength(500);
        
        builder.Property(l => l.Postcode)
            .HasMaxLength(20);
        
        builder.Property(l => l.PropertyType)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(l => l.ServiceType)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(l => l.Timeline)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(l => l.Urgency)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        // Concurrency token
        builder.Property(l => l.Version)
            .IsConcurrencyToken();
        
        // Relationships
        builder.HasOne(l => l.Score)
            .WithOne(s => s.Lead)
            .HasForeignKey<LeadScore>(s => s.LeadId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(l => l.SourceAttribution)
            .WithOne(sa => sa.Lead)
            .HasForeignKey<LeadSourceAttribution>(sa => sa.LeadId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Ignore calculated properties
        builder.Ignore(l => l.FullName);
        builder.Ignore(l => l.IsDeleted);
    }
}
