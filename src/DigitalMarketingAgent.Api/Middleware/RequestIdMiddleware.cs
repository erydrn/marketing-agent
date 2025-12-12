using System.Diagnostics;

namespace DigitalMarketingAgent.Api.Middleware;

/// <summary>
/// Middleware that generates unique request IDs for traceability
/// </summary>
public class RequestIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string RequestIdHeader = "X-Request-ID";

    public RequestIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get or generate request ID
        var requestId = context.Request.Headers[RequestIdHeader].FirstOrDefault() 
            ?? Guid.NewGuid().ToString();

        // Set activity ID for distributed tracing
        Activity.Current?.SetTag("RequestId", requestId);

        // Add to response headers
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[RequestIdHeader] = requestId;
            return Task.CompletedTask;
        });

        // Add to HttpContext items for use in other middleware
        context.Items["RequestId"] = requestId;

        await _next(context);
    }
}
