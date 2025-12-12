namespace DigitalMarketingAgent.Core.DTOs;

/// <summary>
/// Standard error response format
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Problem type URI
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Error title
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    public required int Status { get; init; }

    /// <summary>
    /// Optional validation errors
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; init; }

    /// <summary>
    /// Request trace ID
    /// </summary>
    public string? TraceId { get; init; }

    /// <summary>
    /// Error timestamp
    /// </summary>
    public required DateTime Timestamp { get; init; }

    /// <summary>
    /// Optional additional details
    /// </summary>
    public string? Detail { get; init; }
}
