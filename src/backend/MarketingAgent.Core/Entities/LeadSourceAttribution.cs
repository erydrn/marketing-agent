using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketingAgent.Core.Entities;

/// <summary>
/// Represents the source attribution for a lead capture
/// </summary>
public class LeadSourceAttribution
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Lead))]
    public Guid LeadId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Channel { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Source { get; set; }

    [MaxLength(200)]
    public string? Campaign { get; set; }

    [MaxLength(100)]
    public string? Medium { get; set; }

    [MaxLength(200)]
    public string? UtmSource { get; set; }

    [MaxLength(100)]
    public string? UtmMedium { get; set; }

    [MaxLength(200)]
    public string? UtmCampaign { get; set; }

    [MaxLength(200)]
    public string? UtmContent { get; set; }

    [MaxLength(200)]
    public string? UtmTerm { get; set; }

    [MaxLength(2000)]
    public string? Referrer { get; set; }

    [MaxLength(2000)]
    public string? LandingPage { get; set; }

    [MaxLength(100)]
    public string? IpAddress { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Lead Lead { get; set; } = null!;
}
