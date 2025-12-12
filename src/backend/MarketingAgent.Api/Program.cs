using FluentValidation;
using MarketingAgent.Core.DTOs;
using MarketingAgent.Core.Interfaces;
using MarketingAgent.Core.Validators;
using MarketingAgent.Infrastructure.Data;
using MarketingAgent.Infrastructure.Repositories;
using MarketingAgent.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Marketing Agent API", Version = "v1" });
});

// Add database context (using InMemory for demo, can be changed to SQL Server)
builder.Services.AddDbContext<MarketingAgentDbContext>(options =>
    options.UseInMemoryDatabase("MarketingAgentDb"));

// Add repositories
builder.Services.AddScoped<ILeadRepository, LeadRepository>();

// Add services
builder.Services.AddScoped<ILeadCaptureService, LeadCaptureService>();

// Add validators
builder.Services.AddValidatorsFromAssemblyContaining<WebFormLeadRequestValidator>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Health check endpoints
app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithTags("Health");

// Lead Capture Endpoints
app.MapPost("/api/v1/leads/web-form", async (
    WebFormLeadRequest request,
    ILeadCaptureService leadService,
    IValidator<WebFormLeadRequest> validator,
    HttpContext context) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(new { errors = validationResult.Errors });
    }

    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    var userAgent = context.Request.Headers.UserAgent.ToString();

    var response = await leadService.CaptureWebFormLeadAsync(request, ipAddress, userAgent);
    return Results.Created($"/api/v1/leads/{response.LeadId}", response);
})
.WithName("CaptureWebFormLead")
.WithTags("Lead Capture")
.Produces<LeadCaptureResponse>(201)
.Produces(400);

app.MapPost("/api/v1/leads/ad-platform-webhook", async (
    AdPlatformWebhookRequest request,
    ILeadCaptureService leadService) =>
{
    var response = await leadService.CaptureAdPlatformLeadAsync(request);
    return Results.Ok(response);
})
.WithName("CaptureAdPlatformLead")
.WithTags("Lead Capture")
.Produces<LeadCaptureResponse>(200);

app.MapPost("/api/v1/leads/partner-referral", async (
    PartnerReferralRequest request,
    ILeadCaptureService leadService) =>
{
    var response = await leadService.CapturePartnerReferralAsync(request);
    return Results.Created($"/api/v1/leads/{response.LeadId}", response);
})
.WithName("CapturePartnerReferral")
.WithTags("Lead Capture")
.Produces<LeadCaptureResponse>(201);

app.MapPost("/api/v1/leads/developer-bulk", async (
    DeveloperBulkRequest request,
    ILeadCaptureService leadService) =>
{
    if (request.Leads.Count > 1000)
    {
        return Results.BadRequest(new { error = "Maximum 1000 leads allowed per request" });
    }

    var response = await leadService.CaptureDeveloperBulkLeadsAsync(request);
    return Results.Ok(response);
})
.WithName("CaptureDeveloperBulkLeads")
.WithTags("Lead Capture")
.Produces<BulkLeadCaptureResponse>(200);

app.MapPost("/api/v1/leads/event-registration", async (
    EventRegistrationRequest request,
    ILeadCaptureService leadService) =>
{
    var response = await leadService.CaptureEventRegistrationAsync(request);
    return Results.Ok(response);
})
.WithName("CaptureEventRegistration")
.WithTags("Lead Capture")
.Produces<EventRegistrationResponse>(200);

app.MapGet("/api/v1/leads/{id:guid}", async (
    Guid id,
    ILeadCaptureService leadService) =>
{
    var lead = await leadService.GetLeadByIdAsync(id);
    return lead != null ? Results.Ok(lead) : Results.NotFound();
})
.WithName("GetLeadById")
.WithTags("Leads")
.Produces<LeadResponse>(200)
.Produces(404);

app.MapGet("/api/v1/leads", async (
    ILeadCaptureService leadService,
    int page = 1,
    int pageSize = 30,
    string? source = null,
    string? channel = null,
    DateTime? fromDate = null,
    DateTime? toDate = null) =>
{
    var response = await leadService.GetLeadsAsync(page, pageSize, source, channel, fromDate, toDate);
    return Results.Ok(response);
})
.WithName("GetLeads")
.WithTags("Leads")
.Produces<LeadListResponse>(200);

app.Run();

// Make Program class accessible to tests
public partial class Program { }
