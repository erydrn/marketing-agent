namespace MarketingAgent.Core.DTOs;

/// <summary>
/// Base contact information for all lead submissions
/// </summary>
public record ContactDto(
    string FirstName,
    string LastName,
    string Email,
    string? Phone = null,
    string? PreferredContactMethod = null
);

/// <summary>
/// Property information for lead submissions
/// </summary>
public record PropertyDto(
    string? Address = null,
    string? Postcode = null,
    string? PropertyType = null,
    decimal? EstimatedValue = null
);

/// <summary>
/// Service request information
/// </summary>
public record ServiceRequestDto(
    string ServiceType,
    string? Timeline = null,
    string? Message = null
);

/// <summary>
/// UTM tracking parameters
/// </summary>
public record UtmParamsDto(
    string? Source = null,
    string? Medium = null,
    string? Campaign = null,
    string? Content = null,
    string? Term = null
);

/// <summary>
/// Session data for web form submissions
/// </summary>
public record SessionDataDto(
    List<string>? PagesVisited = null,
    int? TimeOnSite = null
);
