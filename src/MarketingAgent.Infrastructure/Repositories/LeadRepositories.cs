using Microsoft.EntityFrameworkCore;
using MarketingAgent.Core.Entities;
using MarketingAgent.Core.Interfaces;
using MarketingAgent.Infrastructure.Data;

namespace MarketingAgent.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Lead entity
/// </summary>
public class LeadRepository : BaseRepository<Lead>, ILeadRepository
{
    public LeadRepository(MarketingAgentDbContext context) : base(context)
    {
    }
    
    public async Task<Lead?> GetByExternalIdAsync(
        string externalId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(l => l.ExternalLeadId == externalId, cancellationToken);
    }
    
    public async Task<Lead?> GetByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(l => l.Email == email, cancellationToken);
    }
    
    public async Task<IEnumerable<Lead>> GetLeadsWithDetailsAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        
        return await _dbSet
            .Include(l => l.Score)
            .Include(l => l.SourceAttribution)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Lead>> FindPotentialDuplicatesAsync(
        string email, 
        string? phone, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(l => l.Email == email);
        }
        
        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(l => l.Phone == phone);
        }
        
        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Lead>> GetByPostcodeAsync(
        string postcode, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.Postcode == postcode)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for LeadScore entity
/// </summary>
public class LeadScoreRepository : BaseRepository<LeadScore>, ILeadScoreRepository
{
    public LeadScoreRepository(MarketingAgentDbContext context) : base(context)
    {
    }
    
    public async Task<LeadScore?> GetByLeadIdAsync(
        Guid leadId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ls => ls.LeadId == leadId, cancellationToken);
    }
    
    public async Task<IEnumerable<LeadScore>> GetByTierAsync(
        Core.Enums.LeadTier tier, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ls => ls.Tier == tier)
            .Include(ls => ls.Lead)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for LeadSourceAttribution entity
/// </summary>
public class LeadSourceAttributionRepository : BaseRepository<LeadSourceAttribution>, ILeadSourceAttributionRepository
{
    public LeadSourceAttributionRepository(MarketingAgentDbContext context) : base(context)
    {
    }
    
    public async Task<LeadSourceAttribution?> GetByLeadIdAsync(
        Guid leadId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(lsa => lsa.LeadId == leadId, cancellationToken);
    }
    
    public async Task<IEnumerable<LeadSourceAttribution>> GetByChannelAsync(
        Core.Enums.MarketingChannel channel, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(lsa => lsa.Channel == channel)
            .Include(lsa => lsa.Lead)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<LeadSourceAttribution>> GetByCampaignAsync(
        string campaign, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(lsa => lsa.Campaign == campaign)
            .Include(lsa => lsa.Lead)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
