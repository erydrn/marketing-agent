using MarketingAgent.Core.Enums;

namespace MarketingAgent.Core.Entities;

/// <summary>
/// Base entity class providing common properties for all entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Primary key identifier
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Timestamp when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Timestamp when the entity was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Timestamp when the entity was soft deleted (null if not deleted)
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// Version number for optimistic concurrency control
    /// </summary>
    public int Version { get; set; } = 1;
    
    /// <summary>
    /// Indicates if the entity is soft deleted
    /// </summary>
    public bool IsDeleted => DeletedAt.HasValue;
}

/// <summary>
/// Represents a lead captured from various marketing channels
/// </summary>
public class Lead : BaseEntity
{
    /// <summary>
    /// External identifier from the source system (e.g., Google Ads lead ID)
    /// </summary>
    public string ExternalLeadId { get; set; } = string.Empty;
    
    // Contact Information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    
    // Address Information
    public string? Address { get; set; }
    public string? Postcode { get; set; }
    
    // Service Details
    public PropertyType? PropertyType { get; set; }
    public ServiceType? ServiceType { get; set; }
    public Timeline? Timeline { get; set; }
    public Urgency? Urgency { get; set; }
    
    // Related Entities
    public LeadScore? Score { get; set; }
    public LeadSourceAttribution? SourceAttribution { get; set; }
    
    /// <summary>
    /// Full name of the lead
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
