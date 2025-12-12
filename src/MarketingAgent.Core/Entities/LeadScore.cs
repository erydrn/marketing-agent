using MarketingAgent.Core.Enums;

namespace MarketingAgent.Core.Entities;

/// <summary>
/// Represents the scoring and qualification tier for a lead
/// </summary>
public class LeadScore : BaseEntity
{
    /// <summary>
    /// Foreign key to the associated Lead
    /// </summary>
    public Guid LeadId { get; set; }
    
    /// <summary>
    /// Navigation property to the associated Lead
    /// </summary>
    public Lead Lead { get; set; } = null!;
    
    /// <summary>
    /// Overall composite score (0-100)
    /// </summary>
    public int OverallScore { get; set; }
    
    /// <summary>
    /// Classification tier based on overall score
    /// </summary>
    public LeadTier Tier { get; set; }
    
    // Component Scores (each 0-25, total 100)
    
    /// <summary>
    /// Score based on data completeness (0-25)
    /// </summary>
    public int CompletenessScore { get; set; }
    
    /// <summary>
    /// Score based on engagement indicators (0-25)
    /// </summary>
    public int EngagementScore { get; set; }
    
    /// <summary>
    /// Score based on purchase readiness signals (0-25)
    /// </summary>
    public int ReadinessScore { get; set; }
    
    /// <summary>
    /// Score based on source quality (0-25)
    /// </summary>
    public int SourceQualityScore { get; set; }
    
    /// <summary>
    /// Timestamp when the score was calculated
    /// </summary>
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}
