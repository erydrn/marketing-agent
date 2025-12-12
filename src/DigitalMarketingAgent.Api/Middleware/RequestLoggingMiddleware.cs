using System.Diagnostics;

namespace DigitalMarketingAgent.Api.Middleware;

/// <summary>
/// Middleware for logging incoming HTTP requests and responses
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = context.Items["RequestId"]?.ToString() ?? context.TraceIdentifier;
        var startTime = Stopwatch.GetTimestamp();

        // Log incoming request
        _logger.LogInformation(
            "HTTP {Method} {Path} started. RequestId: {RequestId}, RemoteIp: {RemoteIp}",
            context.Request.Method,
            context.Request.Path,
            requestId,
            context.Connection.RemoteIpAddress);

        try
        {
            await _next(context);
        }
        finally
        {
            var elapsedMs = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;

            // Log response
            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {Duration}ms. RequestId: {RequestId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsedMs,
                requestId);
        }
    }
}
