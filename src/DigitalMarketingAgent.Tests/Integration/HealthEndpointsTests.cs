using DigitalMarketingAgent.Core.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace DigitalMarketingAgent.Tests.Integration;

public class HealthEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public HealthEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ShouldReturn200Ok()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task GetHealth_ShouldReturnHealthCheckResponse()
    {
        // Act
        var response = await _client.GetAsync("/health");
        var content = await response.Content.ReadAsStringAsync();
        var healthResponse = JsonSerializer.Deserialize<HealthCheckResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        healthResponse.Should().NotBeNull();
        healthResponse!.Status.Should().Be("ok");
        healthResponse.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetReadiness_ShouldReturn200Ok()
    {
        // Act
        var response = await _client.GetAsync("/health/ready");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetReadiness_ShouldReturnReadinessCheckResponse()
    {
        // Act
        var response = await _client.GetAsync("/health/ready");
        var content = await response.Content.ReadAsStringAsync();
        var healthResponse = JsonSerializer.Deserialize<HealthCheckResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        healthResponse.Should().NotBeNull();
        healthResponse!.Status.Should().Be("ready");
        healthResponse.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        healthResponse.Dependencies.Should().NotBeNull();
        healthResponse.Dependencies.Should().ContainKey("database");
    }

    [Fact]
    public async Task HealthEndpoints_ShouldIncludeRequestIdHeader()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.Headers.Should().ContainKey("X-Request-ID");
        response.Headers.GetValues("X-Request-ID").Should().NotBeEmpty();
    }
}
