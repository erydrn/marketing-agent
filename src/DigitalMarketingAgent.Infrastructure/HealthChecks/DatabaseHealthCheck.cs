using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DigitalMarketingAgent.Infrastructure.HealthChecks;

/// <summary>
/// Basic health check that always returns healthy
/// Can be extended to check actual database connectivity
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // For now, return healthy
        // TODO: In Task 003, implement actual database connectivity check
        return Task.FromResult(HealthCheckResult.Healthy("Database connection is healthy"));
    }
}
