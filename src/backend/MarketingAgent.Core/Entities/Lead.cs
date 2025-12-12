using System;
using System.ComponentModel.DataAnnotations;

namespace MarketingAgent.Core.Entities;

/// <summary>
/// Represents a captured lead from any marketing channel
/// </summary>
public class Lead
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ExternalLeadId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(10)]
    public string? Postcode { get; set; }

    [MaxLength(50)]
    public string? PropertyType { get; set; }

    [Required]
    [MaxLength(50)]
    public string ServiceType { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Timeline { get; set; }

    [MaxLength(5000)]
    public string? Message { get; set; }

    public bool GdprConsent { get; set; }

    public bool MarketingConsent { get; set; }

    [MaxLength(50)]
    public string? PreferredContactMethod { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public int Version { get; set; } = 1;

    // Navigation properties
    public LeadScore? LeadScore { get; set; }
    public ICollection<LeadSourceAttribution> SourceAttributions { get; set; } = new List<LeadSourceAttribution>();
}
