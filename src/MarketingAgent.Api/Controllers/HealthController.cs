using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketingAgent.Infrastructure.Data;

namespace MarketingAgent.Api.Controllers;

/// <summary>
/// Health check endpoints for monitoring system status
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    private readonly MarketingAgentDbContext _dbContext;
    private readonly ILogger<HealthController> _logger;
    
    public HealthController(
        MarketingAgentDbContext dbContext,
        ILogger<HealthController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    /// <summary>
    /// Basic liveness probe
    /// </summary>
    /// <returns>System status</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "ok",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        });
    }
    
    /// <summary>
    /// Readiness probe with dependency checks
    /// </summary>
    /// <returns>Detailed system status including dependencies</returns>
    [HttpGet("ready")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Ready()
    {
        var status = "ready";
        var dependencies = new Dictionary<string, string>();
        
        // Check database connectivity
        try
        {
            await _dbContext.Database.CanConnectAsync();
            dependencies["database"] = "ok";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            dependencies["database"] = "error";
            status = "unavailable";
        }
        
        var response = new
        {
            Status = status,
            Timestamp = DateTime.UtcNow,
            Dependencies = dependencies
        };
        
        if (status == "unavailable")
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, response);
        }
        
        return Ok(response);
    }
}
