using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MarketingAgent.Core.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MarketingAgent.Tests;

public class LeadCaptureApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public LeadCaptureApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CaptureWebFormLead_WithValidData_ReturnsCreated()
    {
        // Arrange
        var request = new WebFormLeadRequest(
            Source: "contact-form",
            PageUrl: "https://example.com/contact",
            UtmParams: null,
            Contact: new ContactDto(
                FirstName: "John",
                LastName: "Smith",
                Email: "john.smith@example.com",
                Phone: "07700900123"
            ),
            Property: null,
            ServiceRequest: new ServiceRequestDto(
                ServiceType: "purchase"
            ),
            GdprConsent: true
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/leads/web-form", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var result = await response.Content.ReadFromJsonAsync<LeadCaptureResponse>();
        result.Should().NotBeNull();
        result!.LeadId.Should().NotBeEmpty();
        result.Status.Should().Be("captured");
    }

    [Fact]
    public async Task CaptureWebFormLead_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var request = new WebFormLeadRequest(
            Source: "contact-form",
            PageUrl: "https://example.com/contact",
            UtmParams: null,
            Contact: new ContactDto(
                FirstName: "John",
                LastName: "Smith",
                Email: "invalid-email",
                Phone: "07700900123"
            ),
            Property: null,
            ServiceRequest: new ServiceRequestDto(
                ServiceType: "purchase"
            ),
            GdprConsent: true
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/leads/web-form", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CaptureWebFormLead_WithoutGdprConsent_ReturnsBadRequest()
    {
        // Arrange
        var request = new WebFormLeadRequest(
            Source: "contact-form",
            PageUrl: "https://example.com/contact",
            UtmParams: null,
            Contact: new ContactDto(
                FirstName: "John",
                LastName: "Smith",
                Email: "john.smith@example.com"
            ),
            Property: null,
            ServiceRequest: new ServiceRequestDto(
                ServiceType: "purchase"
            ),
            GdprConsent: false
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/leads/web-form", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetLeadById_AfterCreation_ReturnsLead()
    {
        // Arrange - Create a lead first
        var createRequest = new WebFormLeadRequest(
            Source: "contact-form",
            PageUrl: "https://example.com/contact",
            UtmParams: null,
            Contact: new ContactDto(
                FirstName: "Jane",
                LastName: "Doe",
                Email: "jane.doe@example.com"
            ),
            Property: null,
            ServiceRequest: new ServiceRequestDto(
                ServiceType: "sale"
            ),
            GdprConsent: true
        );

        var createResponse = await _client.PostAsJsonAsync("/api/v1/leads/web-form", createRequest);
        var captureResult = await createResponse.Content.ReadFromJsonAsync<LeadCaptureResponse>();

        // Act - Get the created lead
        var getResponse = await _client.GetAsync($"/api/v1/leads/{captureResult!.LeadId}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var lead = await getResponse.Content.ReadFromJsonAsync<LeadResponse>();
        lead.Should().NotBeNull();
        lead!.Email.Should().Be("jane.doe@example.com");
        lead.FirstName.Should().Be("Jane");
        lead.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetLeads_ReturnsLeadList()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/leads?page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<LeadListResponse>();
        result.Should().NotBeNull();
        result!.Leads.Should().NotBeNull();
    }
}

// Make Program class accessible to tests
public partial class Program { }
