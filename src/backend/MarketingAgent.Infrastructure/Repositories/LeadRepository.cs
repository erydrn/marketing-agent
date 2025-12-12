using Microsoft.EntityFrameworkCore;
using MarketingAgent.Core.Entities;
using MarketingAgent.Core.Interfaces;
using MarketingAgent.Infrastructure.Data;

namespace MarketingAgent.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Lead entity
/// </summary>
public class LeadRepository : ILeadRepository
{
    private readonly MarketingAgentDbContext _context;

    public LeadRepository(MarketingAgentDbContext context)
    {
        _context = context;
    }

    public async Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Leads
            .Include(l => l.LeadScore)
            .Include(l => l.SourceAttributions)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Lead?> GetByEmailAndPhoneAsync(string email, string? phone, CancellationToken cancellationToken = default)
    {
        var query = _context.Leads
            .Include(l => l.LeadScore)
            .Include(l => l.SourceAttributions)
            .Where(l => l.Email == email);

        if (!string.IsNullOrEmpty(phone))
        {
            query = query.Where(l => l.Phone == phone);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(List<Lead> Leads, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? source = null,
        string? channel = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Leads
            .Include(l => l.LeadScore)
            .Include(l => l.SourceAttributions)
            .AsQueryable();

        if (!string.IsNullOrEmpty(channel))
        {
            query = query.Where(l => l.SourceAttributions.Any(a => a.Channel == channel));
        }

        if (!string.IsNullOrEmpty(source))
        {
            query = query.Where(l => l.SourceAttributions.Any(a => a.Source == source));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(l => l.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(l => l.CreatedAt <= toDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var leads = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (leads, totalCount);
    }

    public async Task<Lead> CreateAsync(Lead lead, CancellationToken cancellationToken = default)
    {
        lead.CreatedAt = DateTime.UtcNow;
        lead.UpdatedAt = DateTime.UtcNow;
        
        _context.Leads.Add(lead);
        await _context.SaveChangesAsync(cancellationToken);
        
        return lead;
    }

    public async Task<Lead> UpdateAsync(Lead lead, CancellationToken cancellationToken = default)
    {
        lead.UpdatedAt = DateTime.UtcNow;
        lead.Version++;
        
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync(cancellationToken);
        
        return lead;
    }

    public async Task<bool> ExistsAsync(string email, string? phone, DateTime sinceDate, CancellationToken cancellationToken = default)
    {
        var query = _context.Leads
            .Where(l => l.Email == email && l.CreatedAt >= sinceDate);

        if (!string.IsNullOrEmpty(phone))
        {
            query = query.Where(l => l.Phone == phone);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
