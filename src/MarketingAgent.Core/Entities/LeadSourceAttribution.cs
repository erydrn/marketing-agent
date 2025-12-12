using MarketingAgent.Core.Enums;

namespace MarketingAgent.Core.Entities;

/// <summary>
/// Tracks the source attribution for a lead across marketing channels
/// </summary>
public class LeadSourceAttribution : BaseEntity
{
    /// <summary>
    /// Foreign key to the associated Lead
    /// </summary>
    public Guid LeadId { get; set; }
    
    /// <summary>
    /// Navigation property to the associated Lead
    /// </summary>
    public Lead Lead { get; set; } = null!;
    
    // Channel Information
    
    /// <summary>
    /// Primary marketing channel
    /// </summary>
    public MarketingChannel Channel { get; set; }
    
    /// <summary>
    /// Specific source within the channel (e.g., "Google", "Facebook")
    /// </summary>
    public string? Source { get; set; }
    
    /// <summary>
    /// Campaign identifier or name
    /// </summary>
    public string? Campaign { get; set; }
    
    /// <summary>
    /// Medium of the marketing channel (e.g., "cpc", "email", "social")
    /// </summary>
    public string? Medium { get; set; }
    
    // UTM Parameters (for web traffic)
    
    /// <summary>
    /// UTM source parameter
    /// </summary>
    public string? UtmSource { get; set; }
    
    /// <summary>
    /// UTM medium parameter
    /// </summary>
    public string? UtmMedium { get; set; }
    
    /// <summary>
    /// UTM campaign parameter
    /// </summary>
    public string? UtmCampaign { get; set; }
    
    /// <summary>
    /// UTM content parameter (for A/B testing)
    /// </summary>
    public string? UtmContent { get; set; }
    
    /// <summary>
    /// UTM term parameter (for paid search keywords)
    /// </summary>
    public string? UtmTerm { get; set; }
    
    // Additional Tracking
    
    /// <summary>
    /// HTTP referrer URL
    /// </summary>
    public string? Referrer { get; set; }
    
    /// <summary>
    /// Landing page URL where lead was captured
    /// </summary>
    public string? LandingPage { get; set; }
    
    /// <summary>
    /// Timestamp when the lead was captured
    /// </summary>
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
}
