using Serilog;
using Microsoft.EntityFrameworkCore;
using MarketingAgent.Infrastructure.Data;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/marketing-agent-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Marketing Agent API");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog as the logging provider
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();

    // Configure Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Marketing Agent API",
            Version = "v1",
            Description = "Digital Marketing Agent for lead capture, qualification, and routing",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Marketing Agent Team"
            }
        });
    });

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Add Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<MarketingAgentDbContext>("database");

    // Configure Database
    builder.Services.AddDbContext<MarketingAgentDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Use in-memory database for development if no connection string
            Log.Warning("No database connection string found. Using in-memory database.");
            options.UseInMemoryDatabase("MarketingAgentDb");
        }
        else
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(60);
            });
        }

        // Enable sensitive data logging in development
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marketing Agent API v1");
            c.RoutePrefix = "api/docs";
        });
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseCors("AllowAll");
    app.UseAuthorization();
    
    app.MapControllers();
    
    // Map health check endpoints
    app.MapHealthChecks("/health");
    app.MapHealthChecks("/health/ready");

    // Root endpoint
    app.MapGet("/", () => new
    {
        Name = "Marketing Agent API",
        Version = "1.0.0",
        Status = "Running",
        Documentation = "/api/docs"
    })
    .WithName("Root")
    .WithTags("Info");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
