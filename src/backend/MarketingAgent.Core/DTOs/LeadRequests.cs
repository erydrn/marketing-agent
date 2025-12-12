namespace MarketingAgent.Core.DTOs;

/// <summary>
/// Request DTO for web form lead submissions
/// </summary>
public record WebFormLeadRequest(
    string Source,
    string PageUrl,
    UtmParamsDto? UtmParams,
    ContactDto Contact,
    PropertyDto? Property,
    ServiceRequestDto ServiceRequest,
    bool GdprConsent,
    bool? MarketingConsent = false,
    string? Referrer = null,
    SessionDataDto? SessionData = null
);

/// <summary>
/// Request DTO for ad platform webhook submissions
/// </summary>
public record AdPlatformWebhookRequest(
    string Platform,
    string CampaignId,
    string CampaignName,
    string? AdGroupId,
    string? AdId,
    string FormId,
    DateTime SubmittedAt,
    ContactDto Contact,
    Dictionary<string, object>? CustomFields,
    string PlatformLeadId
);

/// <summary>
/// Request DTO for partner referral submissions
/// </summary>
public record PartnerReferralRequest(
    string PartnerId,
    string PartnerName,
    string ReferralType,
    ContactDto Contact,
    PropertyDto? Property,
    ServiceRequestDto ServiceRequest,
    ReferralAgreementDto? ReferralAgreement = null
);

/// <summary>
/// Referral agreement information
/// </summary>
public record ReferralAgreementDto(
    decimal? CommissionRate = null,
    string? CommissionType = null
);

/// <summary>
/// Request DTO for developer bulk upload
/// </summary>
public record DeveloperBulkRequest(
    string DeveloperId,
    string DevelopmentName,
    string? DevelopmentLocation,
    List<DeveloperLeadDto> Leads
);

/// <summary>
/// Individual lead in developer bulk upload
/// </summary>
public record DeveloperLeadDto(
    ContactDto Contact,
    DeveloperPropertyDto Property,
    ServiceRequestDto ServiceRequest
);

/// <summary>
/// Property details for developer leads
/// </summary>
public record DeveloperPropertyDto(
    string? DevelopmentUnit = null,
    string? PlotNumber = null,
    string? PropertyType = null,
    decimal? Price = null,
    string? PurchaseStage = null
);

/// <summary>
/// Request DTO for event registration submissions
/// </summary>
public record EventRegistrationRequest(
    string EventType,
    string EventName,
    DateTime? EventDate,
    List<EventRegistrantDto> Registrants
);

/// <summary>
/// Individual registrant in event registration
/// </summary>
public record EventRegistrantDto(
    ContactDto Contact,
    string? Company = null,
    string? JobTitle = null,
    List<string>? Interests = null,
    List<string>? Questions = null
);
