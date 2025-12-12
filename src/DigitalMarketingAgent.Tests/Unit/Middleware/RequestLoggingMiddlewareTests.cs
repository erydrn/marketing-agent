using DigitalMarketingAgent.Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace DigitalMarketingAgent.Tests.Unit.Middleware;

public class RequestLoggingMiddlewareTests
{
    private readonly Mock<ILogger<RequestLoggingMiddleware>> _loggerMock;
    private readonly RequestLoggingMiddleware _middleware;

    public RequestLoggingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
        _middleware = new RequestLoggingMiddleware(
            context => Task.CompletedTask,
            _loggerMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_ShouldLogRequestAndResponse()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/api/test";
        context.Response.StatusCode = 200;
        context.Items["RequestId"] = "test-request-id";

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("GET") && v.ToString()!.Contains("/api/test") && v.ToString()!.Contains("started")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("GET") && v.ToString()!.Contains("/api/test") && v.ToString()!.Contains("responded") && v.ToString()!.Contains("200")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldLogDuration()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = "/api/leads";
        context.Response.StatusCode = 201;
        context.Items["RequestId"] = "test-request-id";

        var middleware = new RequestLoggingMiddleware(
            async _ => await Task.Delay(100), // Simulate some processing time
            _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("responded") && v.ToString()!.Contains("ms")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
