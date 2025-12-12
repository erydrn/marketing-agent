using DigitalMarketingAgent.Api.Middleware;
using DigitalMarketingAgent.Core.DTOs;
using DigitalMarketingAgent.Core.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace DigitalMarketingAgent.Tests.Unit.Middleware;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly ErrorHandlingMiddleware _middleware;

    public ErrorHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _middleware = new ErrorHandlingMiddleware(
            context => Task.CompletedTask,
            _loggerMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoException_ShouldCallNext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextCalled = false;
        var middleware = new ErrorHandlingMiddleware(
            _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            },
            _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationException_ShouldReturn400WithErrors()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var errors = new Dictionary<string, string[]>
        {
            { "email", new[] { "Email is required" } }
        };

        var middleware = new ErrorHandlingMiddleware(
            _ => throw new ValidationException(errors),
            _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(400);
        context.Response.ContentType.Should().Be("application/json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        errorResponse.Should().NotBeNull();
        errorResponse!.Status.Should().Be(400);
        errorResponse.Title.Should().Be("Validation Error");
        errorResponse.Errors.Should().ContainKey("email");
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundException_ShouldReturn404()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var middleware = new ErrorHandlingMiddleware(
            _ => throw new NotFoundException("Lead", "123"),
            _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(404);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        errorResponse.Should().NotBeNull();
        errorResponse!.Status.Should().Be(404);
        errorResponse.Title.Should().Be("Not Found");
    }

    [Fact]
    public async Task InvokeAsync_WhenExternalServiceException_ShouldReturn503()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var middleware = new ErrorHandlingMiddleware(
            _ => throw new ExternalServiceException("SalesAgent", "Connection timeout"),
            _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(503);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        errorResponse.Should().NotBeNull();
        errorResponse!.Status.Should().Be(503);
        errorResponse.Title.Should().Be("External Service Error");
    }

    [Fact]
    public async Task InvokeAsync_WhenUnhandledException_ShouldReturn500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var middleware = new ErrorHandlingMiddleware(
            _ => throw new InvalidOperationException("Unexpected error"),
            _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(500);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        errorResponse.Should().NotBeNull();
        errorResponse!.Status.Should().Be(500);
        errorResponse.Title.Should().Be("Internal Server Error");
        errorResponse.Detail.Should().Be("An unexpected error occurred. Please try again later.");
    }
}
