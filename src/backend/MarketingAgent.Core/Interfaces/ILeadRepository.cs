using MarketingAgent.Core.Entities;

namespace MarketingAgent.Core.Interfaces;

/// <summary>
/// Repository interface for Lead entity operations
/// </summary>
public interface ILeadRepository
{
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Lead?> GetByEmailAndPhoneAsync(string email, string? phone, CancellationToken cancellationToken = default);
    Task<(List<Lead> Leads, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? source = null,
        string? channel = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
    Task<Lead> CreateAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<Lead> UpdateAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, string? phone, DateTime sinceDate, CancellationToken cancellationToken = default);
}
