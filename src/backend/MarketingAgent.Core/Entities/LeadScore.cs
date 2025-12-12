using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketingAgent.Core.Entities;

/// <summary>
/// Represents the scoring information for a lead
/// </summary>
public class LeadScore
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Lead))]
    public Guid LeadId { get; set; }

    [Range(0, 100)]
    public int OverallScore { get; set; }

    [Required]
    [MaxLength(20)]
    public string Tier { get; set; } = "Cold";

    [Range(0, 25)]
    public int CompletenessScore { get; set; }

    [Range(0, 25)]
    public int EngagementScore { get; set; }

    [Range(0, 25)]
    public int ReadinessScore { get; set; }

    [Range(0, 25)]
    public int SourceQualityScore { get; set; }

    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Lead Lead { get; set; } = null!;
}
