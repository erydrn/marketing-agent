using DigitalMarketingAgent.Core.DTOs;

namespace DigitalMarketingAgent.Api.Endpoints;

/// <summary>
/// Health check endpoints
/// </summary>
public static class HealthEndpoints
{
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/health", GetHealth)
            .WithName("GetHealth")
            .WithDescription("Basic liveness check")
            .WithSummary("Health check")
            .Produces<HealthCheckResponse>(StatusCodes.Status200OK);

        group.MapGet("/health/ready", GetReadiness)
            .WithName("GetReadiness")
            .WithDescription("Readiness check with dependency validation")
            .WithSummary("Readiness check")
            .Produces<HealthCheckResponse>(StatusCodes.Status200OK)
            .Produces<HealthCheckResponse>(StatusCodes.Status503ServiceUnavailable);

        return group;
    }

    private static IResult GetHealth()
    {
        var response = new HealthCheckResponse
        {
            Status = "ok",
            Timestamp = DateTime.UtcNow
        };

        return Results.Ok(response);
    }

    private static async Task<IResult> GetReadiness(
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService healthCheckService)
    {
        var healthReport = await healthCheckService.CheckHealthAsync();

        var dependencies = healthReport.Entries.ToDictionary(
            entry => entry.Key,
            entry => entry.Value.Status.ToString().ToLowerInvariant()
        );

        var status = healthReport.Status switch
        {
            Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => "ready",
            Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => "degraded",
            _ => "unavailable"
        };

        var response = new HealthCheckResponse
        {
            Status = status,
            Timestamp = DateTime.UtcNow,
            Dependencies = dependencies
        };

        var statusCode = healthReport.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy
            ? StatusCodes.Status200OK
            : StatusCodes.Status503ServiceUnavailable;

        return Results.Json(response, statusCode: statusCode);
    }
}
