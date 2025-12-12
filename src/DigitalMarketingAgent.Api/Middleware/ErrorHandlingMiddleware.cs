using DigitalMarketingAgent.Core.DTOs;
using DigitalMarketingAgent.Core.Exceptions;
using System.Diagnostics;
using System.Text.Json;

namespace DigitalMarketingAgent.Api.Middleware;

/// <summary>
/// Global error handling middleware
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        
        ErrorResponse response;
        
        switch (exception)
        {
            case ValidationException validationEx:
                response = new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = "Validation Error",
                    Status = validationEx.StatusCode,
                    Errors = validationEx.Errors,
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning(validationEx, 
                    "Validation error: {Message}. TraceId: {TraceId}", 
                    validationEx.Message, traceId);
                break;

            case NotFoundException notFoundEx:
                response = new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    Title = "Not Found",
                    Status = notFoundEx.StatusCode,
                    Detail = notFoundEx.Message,
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning(notFoundEx, 
                    "Resource not found: {Message}. TraceId: {TraceId}", 
                    notFoundEx.Message, traceId);
                break;

            case ExternalServiceException serviceEx:
                response = new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.6.4",
                    Title = "External Service Error",
                    Status = serviceEx.StatusCode,
                    Detail = $"Failed to communicate with {serviceEx.ServiceName}",
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogError(serviceEx, 
                    "External service error: {ServiceName}. TraceId: {TraceId}", 
                    serviceEx.ServiceName, traceId);
                break;

            case ApiException apiEx:
                response = new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                    Title = "API Error",
                    Status = apiEx.StatusCode,
                    Detail = apiEx.Message,
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogError(apiEx, 
                    "API error: {Message}. TraceId: {TraceId}", 
                    apiEx.Message, traceId);
                break;

            default:
                response = new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                    Title = "Internal Server Error",
                    Status = 500,
                    Detail = "An unexpected error occurred. Please try again later.",
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogError(exception, 
                    "Unhandled exception: {Message}. TraceId: {TraceId}", 
                    exception.Message, traceId);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.Status;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
