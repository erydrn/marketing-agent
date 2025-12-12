namespace MarketingAgent.Core.Enums;

/// <summary>
/// Classification tier for leads based on their overall score
/// </summary>
public enum LeadTier
{
    /// <summary>
    /// High priority leads (score 75-100) - immediate action required
    /// </summary>
    Hot,
    
    /// <summary>
    /// Good quality leads (score 50-74) - action within 24 hours
    /// </summary>
    Warm,
    
    /// <summary>
    /// Medium quality leads (score 25-49) - action within 48 hours
    /// </summary>
    Cool,
    
    /// <summary>
    /// Low quality leads (score 0-24) - nurture or disqualify
    /// </summary>
    Cold
}

/// <summary>
/// Property types for real estate services
/// </summary>
public enum PropertyType
{
    Residential,
    Commercial,
    Land,
    Industrial,
    Mixed,
    Other
}

/// <summary>
/// Service types offered
/// </summary>
public enum ServiceType
{
    Conveyancing,
    Remortgage,
    Transfer,
    LeaseholdExtension,
    NewBuild,
    Auction,
    Other
}

/// <summary>
/// Timeline urgency for service completion
/// </summary>
public enum Timeline
{
    Immediate,      // Within 1-2 weeks
    Urgent,         // Within 1 month
    Standard,       // 1-3 months
    Flexible        // 3+ months
}

/// <summary>
/// Customer urgency level
/// </summary>
public enum Urgency
{
    Critical,       // Needs immediate attention
    High,           // Important, act soon
    Medium,         // Standard processing
    Low             // No rush
}

/// <summary>
/// Marketing channel types
/// </summary>
public enum MarketingChannel
{
    DigitalAds,
    PR,
    BusinessDevelopment,
    OrganicMarketing,
    Referral,
    Direct,
    Other
}
