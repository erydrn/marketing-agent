using DigitalMarketingAgent.Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace DigitalMarketingAgent.Tests.Unit.Middleware;

public class RequestIdMiddlewareTests
{
    private readonly RequestIdMiddleware _middleware;

    public RequestIdMiddlewareTests()
    {
        _middleware = new RequestIdMiddleware(context => Task.CompletedTask);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoRequestIdProvided_ShouldGenerateNewId()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Items.Should().ContainKey("RequestId");
        context.Items["RequestId"].Should().NotBeNull();
        context.Items["RequestId"].Should().BeOfType<string>();
        
        var requestId = context.Items["RequestId"] as string;
        Guid.TryParse(requestId, out _).Should().BeTrue("RequestId should be a valid GUID");
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestIdProvided_ShouldUseProvidedId()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var providedRequestId = "custom-request-id-123";
        context.Request.Headers["X-Request-ID"] = providedRequestId;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Items["RequestId"].Should().Be(providedRequestId);
    }
}
