using MarketingAgent.Core.Entities;

namespace MarketingAgent.Core.Interfaces;

/// <summary>
/// Specialized repository interface for Lead entity with additional query methods
/// </summary>
public interface ILeadRepository : IRepository<Lead>
{
    /// <summary>
    /// Get a lead by its external ID from the source system
    /// </summary>
    Task<Lead?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get a lead by email address
    /// </summary>
    Task<Lead?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get leads with their scores and source attribution
    /// </summary>
    Task<IEnumerable<Lead>> GetLeadsWithDetailsAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Find potential duplicate leads by email or phone
    /// </summary>
    Task<IEnumerable<Lead>> FindPotentialDuplicatesAsync(
        string email, 
        string? phone, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get leads by postcode
    /// </summary>
    Task<IEnumerable<Lead>> GetByPostcodeAsync(
        string postcode, 
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for LeadScore entity
/// </summary>
public interface ILeadScoreRepository : IRepository<LeadScore>
{
    /// <summary>
    /// Get the score for a specific lead
    /// </summary>
    Task<LeadScore?> GetByLeadIdAsync(Guid leadId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all scores for a specific tier
    /// </summary>
    Task<IEnumerable<LeadScore>> GetByTierAsync(
        Enums.LeadTier tier, 
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for LeadSourceAttribution entity
/// </summary>
public interface ILeadSourceAttributionRepository : IRepository<LeadSourceAttribution>
{
    /// <summary>
    /// Get source attribution for a specific lead
    /// </summary>
    Task<LeadSourceAttribution?> GetByLeadIdAsync(
        Guid leadId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all attributions for a specific channel
    /// </summary>
    Task<IEnumerable<LeadSourceAttribution>> GetByChannelAsync(
        Enums.MarketingChannel channel, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all attributions for a specific campaign
    /// </summary>
    Task<IEnumerable<LeadSourceAttribution>> GetByCampaignAsync(
        string campaign, 
        CancellationToken cancellationToken = default);
}
