using MarketingAgent.Core.DTOs;

namespace MarketingAgent.Core.Interfaces;

/// <summary>
/// Service interface for lead capture operations
/// </summary>
public interface ILeadCaptureService
{
    Task<LeadCaptureResponse> CaptureWebFormLeadAsync(WebFormLeadRequest request, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
    Task<LeadCaptureResponse> CaptureAdPlatformLeadAsync(AdPlatformWebhookRequest request, CancellationToken cancellationToken = default);
    Task<LeadCaptureResponse> CapturePartnerReferralAsync(PartnerReferralRequest request, CancellationToken cancellationToken = default);
    Task<BulkLeadCaptureResponse> CaptureDeveloperBulkLeadsAsync(DeveloperBulkRequest request, CancellationToken cancellationToken = default);
    Task<EventRegistrationResponse> CaptureEventRegistrationAsync(EventRegistrationRequest request, CancellationToken cancellationToken = default);
    Task<LeadResponse?> GetLeadByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LeadListResponse> GetLeadsAsync(int page, int pageSize, string? source, string? channel, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
}
