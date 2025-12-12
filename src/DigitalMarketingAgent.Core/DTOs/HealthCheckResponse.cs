namespace DigitalMarketingAgent.Core.DTOs;

/// <summary>
/// Health check response model
/// </summary>
public class HealthCheckResponse
{
    /// <summary>
    /// Overall status: "ok", "ready", "degraded", or "unavailable"
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Timestamp of health check
    /// </summary>
    public required DateTime Timestamp { get; init; }

    /// <summary>
    /// Optional dependency health status
    /// </summary>
    public Dictionary<string, string>? Dependencies { get; init; }
}
