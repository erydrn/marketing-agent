namespace MarketingAgent.Core.DTOs;

/// <summary>
/// Standard response for single lead capture
/// </summary>
public record LeadCaptureResponse(
    Guid LeadId,
    string Status,
    DateTime CapturedAt
);

/// <summary>
/// Response for bulk lead upload
/// </summary>
public record BulkLeadCaptureResponse(
    int Processed,
    int Successful,
    int Failed,
    List<Guid> LeadIds,
    List<BulkLeadError>? Errors = null
);

/// <summary>
/// Error information for bulk uploads
/// </summary>
public record BulkLeadError(
    int Index,
    string Message,
    Dictionary<string, List<string>>? ValidationErrors = null
);

/// <summary>
/// Response for event registration
/// </summary>
public record EventRegistrationResponse(
    int Registered,
    List<Guid> LeadIds
);

/// <summary>
/// Response for retrieving a single lead
/// </summary>
public record LeadResponse(
    Guid Id,
    string ExternalLeadId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    string? Postcode,
    string? PropertyType,
    string ServiceType,
    string? Timeline,
    string? Message,
    bool GdprConsent,
    bool MarketingConsent,
    string? PreferredContactMethod,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    LeadScoreResponse? Score,
    List<LeadSourceAttributionResponse> SourceAttributions
);

/// <summary>
/// Response for lead score
/// </summary>
public record LeadScoreResponse(
    int OverallScore,
    string Tier,
    int CompletenessScore,
    int EngagementScore,
    int ReadinessScore,
    int SourceQualityScore,
    DateTime CalculatedAt
);

/// <summary>
/// Response for lead source attribution
/// </summary>
public record LeadSourceAttributionResponse(
    string Channel,
    string? Source,
    string? Campaign,
    string? Medium,
    string? UtmSource,
    string? UtmMedium,
    string? UtmCampaign,
    string? UtmContent,
    string? UtmTerm,
    string? Referrer,
    string? LandingPage,
    DateTime CapturedAt
);

/// <summary>
/// Response for lead list with pagination
/// </summary>
public record LeadListResponse(
    List<LeadResponse> Leads,
    int TotalCount,
    int Page,
    int PageSize
);
