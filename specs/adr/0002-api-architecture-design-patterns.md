# ADR-0002: API Architecture and Design Patterns

**Date**: 2025-12-12  
**Status**: Proposed

## Context

With .NET 9.0 selected as the backend technology stack (ADR-0001), we need to define the API architecture, design patterns, and structural conventions for the Digital Marketing Agent backend. This decision impacts:

- API versioning and evolution
- Request/response patterns
- Error handling strategy
- Middleware architecture
- Project organization
- Testing approach

The backend will serve multiple client types:
- Frontend dashboard (real-time updates)
- External integrations (Google Ads, Sales Agent, Lead System)
- Webhook receivers (conversion feedback)
- Scheduled jobs (analytics aggregation)

## Decision Drivers

1. **API Evolution**: Support future API changes without breaking existing clients
2. **Developer Experience**: Clear, consistent API patterns
3. **OpenAPI Compliance**: Generate comprehensive API documentation
4. **Testability**: Easy to unit test and integration test
5. **Maintainability**: Clear separation of concerns
6. **Performance**: Minimal overhead, efficient request processing
7. **Consistency**: Standardized error responses, status codes
8. **Extensibility**: Easy to add new endpoints and features

## Considered Options

### Option 1: ASP.NET Core Minimal APIs with Clean Architecture

**Description**: Use .NET 9 minimal APIs with Clean Architecture (Core/Application/Infrastructure layers), REST conventions, and result pattern for error handling.

**Pros**:
- ✅ **Minimal Boilerplate**: Less ceremony than traditional controllers
- ✅ **Clean Architecture**: Clear separation between domain, application, and infrastructure
- ✅ **Testability**: Business logic independent of framework
- ✅ **Dependency Inversion**: Interfaces in Core, implementations in Infrastructure
- ✅ **Modern .NET**: Embraces latest .NET patterns and performance optimizations
- ✅ **Type Safety**: Strong typing throughout the stack
- ✅ **OpenAPI**: First-class support via `.WithOpenApi()`
- ✅ **Performance**: Reduced memory allocation, faster startup

**Cons**:
- ⚠️ **New Pattern**: Team may be more familiar with traditional controllers
- ⚠️ **Less Structure**: Requires discipline to maintain organization
- ⚠️ **Middleware Order**: Manual configuration of pipeline order

---

### Option 2: Traditional MVC Controllers with N-Layer Architecture

**Description**: Use ASP.NET Core MVC controllers with traditional N-layer architecture (Controllers → Services → Repositories).

**Pros**:
- ✅ **Familiar Pattern**: Well-understood by most .NET developers
- ✅ **Structured**: Clear controller/action organization
- ✅ **Attributes**: Rich attribute-based routing and validation
- ✅ **Convention-Based**: Follows established MVC conventions

**Cons**:
- ⚠️ **More Boilerplate**: Verbose controller classes
- ⚠️ **Coupling**: Controllers often become tightly coupled to framework
- ⚠️ **Performance**: Slight overhead compared to minimal APIs
- ⚠️ **Legacy Pattern**: .NET team recommending minimal APIs for new projects
- ⚠️ **Testing**: Controllers harder to test due to framework dependencies

---

### Option 3: Vertical Slice Architecture with Minimal APIs

**Description**: Organize code by feature (vertical slices) rather than technical layers, using minimal APIs and MediatR for request handling.

**Pros**:
- ✅ **Feature Cohesion**: All code for a feature in one place
- ✅ **Reduced Coupling**: Features are independent
- ✅ **Easy to Understand**: Clear feature boundaries
- ✅ **MediatR**: Clean command/query separation (CQRS)

**Cons**:
- ⚠️ **Learning Curve**: Less familiar pattern
- ⚠️ **Duplication Risk**: Shared logic may be duplicated across slices
- ⚠️ **MediatR Overhead**: Additional abstraction layer
- ⚠️ **Complex for Simple APIs**: Overkill for straightforward CRUD operations

---

## Decision Outcome

**Chosen Option**: **ASP.NET Core Minimal APIs with Clean Architecture**

**Rationale**:

1. **Modern .NET Best Practice**: Microsoft recommends minimal APIs for new .NET applications. This positions the project for long-term maintainability.

2. **Performance**: Minimal APIs offer better performance than traditional controllers, critical for the 99.5% uptime SLA.

3. **Clean Architecture Alignment**: Separates business logic (Core) from infrastructure concerns, making the system testable and adaptable. If Sales Agent API changes, only Infrastructure layer changes.

4. **Testability**: Core business logic (lead scoring, qualification, routing) can be tested independently of ASP.NET Core framework.

5. **Simplicity**: For an API-first backend with ~10-15 endpoints (Phase 1), minimal APIs provide sufficient structure without controller overhead.

6. **OpenAPI Support**: Built-in `.WithOpenApi()` extension methods make API documentation trivial (Task 002 requirement).

7. **Flexibility**: Easy to add new endpoints as features evolve through Phase 2 and 3.

## Consequences

### Positive

- ✅ **High Performance**: Reduced allocations and faster request processing
- ✅ **Clean Separation**: Business logic isolated from framework concerns
- ✅ **Easy Testing**: Unit test Core logic without ASP.NET dependencies
- ✅ **Modern Codebase**: Aligns with .NET 9 best practices
- ✅ **OpenAPI Excellence**: First-class OpenAPI generation
- ✅ **Dependency Injection**: Natural DI usage throughout
- ✅ **Minimal Boilerplate**: Focus on business logic, not framework ceremony

### Negative

- ⚠️ **Learning Curve**: Team needs to understand minimal APIs and Clean Architecture
  - *Mitigation*: Provide reference examples, leverage GitHub Copilot, create templates
- ⚠️ **Manual Organization**: Requires discipline in endpoint organization
  - *Mitigation*: Use endpoint groups, clear folder structure, naming conventions
- ⚠️ **Less Scaffolding**: No controller scaffolding tools
  - *Mitigation*: Create custom templates/snippets for common patterns

### Neutral

- 📝 **Different from MVC**: Team familiar with MVC may need adjustment period
- 📝 **Explicit Pipeline**: Middleware order must be explicit in Program.cs

## Implementation Notes

### Project Structure (Clean Architecture)

```
src/
├── DigitalMarketingAgent.Api/
│   ├── Program.cs                      # Application entry point, middleware pipeline
│   ├── appsettings.json                # Configuration
│   ├── appsettings.Development.json
│   ├── Endpoints/                      # Minimal API endpoint definitions
│   │   ├── LeadEndpoints.cs            # Lead capture, retrieval endpoints
│   │   ├── HealthEndpoints.cs          # Health check endpoints
│   │   └── WebhookEndpoints.cs         # Sales Agent feedback webhooks
│   ├── Middleware/                     # Custom middleware
│   │   ├── ErrorHandlingMiddleware.cs
│   │   ├── RequestLoggingMiddleware.cs
│   │   └── RequestIdMiddleware.cs
│   ├── Filters/                        # Endpoint filters
│   │   └── ValidationFilter.cs
│   └── Extensions/                     # Service registration extensions
│       ├── ServiceCollectionExtensions.cs
│       └── WebApplicationExtensions.cs
│
├── DigitalMarketingAgent.Core/         # Business logic (framework-agnostic)
│   ├── Models/                         # Domain models
│   │   ├── Lead.cs
│   │   ├── Source.cs
│   │   ├── Channel.cs
│   │   └── LeadScore.cs
│   ├── Interfaces/                     # Abstractions
│   │   ├── ILeadService.cs
│   │   ├── ILeadRepository.cs
│   │   ├── IScoringEngine.cs
│   │   └── ISalesAgentClient.cs
│   ├── Services/                       # Business services
│   │   ├── LeadService.cs
│   │   ├── ScoringEngine.cs
│   │   └── QualificationService.cs
│   ├── DTOs/                           # Data transfer objects
│   │   ├── CreateLeadRequest.cs
│   │   ├── LeadResponse.cs
│   │   └── HealthCheckResponse.cs
│   ├── Validators/                     # FluentValidation validators
│   │   └── CreateLeadRequestValidator.cs
│   ├── Exceptions/                     # Custom exceptions
│   │   ├── ValidationException.cs
│   │   ├── NotFoundException.cs
│   │   └── ExternalServiceException.cs
│   └── Common/                         # Shared utilities
│       └── Result.cs                   # Result pattern for error handling
│
├── DigitalMarketingAgent.Infrastructure/
│   ├── Data/                           # Data access
│   │   ├── ApplicationDbContext.cs     # EF Core context (if using database)
│   │   ├── Repositories/
│   │   │   └── LeadRepository.cs
│   │   └── Configurations/             # EF entity configurations
│   │       └── LeadConfiguration.cs
│   ├── Integrations/                   # External API clients
│   │   ├── SalesAgentClient.cs
│   │   ├── GoogleAdsClient.cs
│   │   └── LeadSystemClient.cs
│   ├── Services/                       # Infrastructure services
│   │   ├── EmailParsingService.cs
│   │   └── PostcodeLookupService.cs
│   └── Configuration/                  # Startup configuration
│       └── DependencyInjection.cs      # Infrastructure service registration
│
└── DigitalMarketingAgent.Tests/
    ├── Unit/                           # Unit tests
    │   ├── Core/
    │   │   └── Services/
    │   │       └── ScoringEngineTests.cs
    │   └── Api/
    │       └── Middleware/
    │           └── ErrorHandlingMiddlewareTests.cs
    └── Integration/                    # Integration tests
        └── Endpoints/
            └── LeadEndpointsTests.cs
```

### API Conventions

#### REST Principles
- `POST /api/leads` - Create lead (201 Created)
- `GET /api/leads/{id}` - Retrieve lead (200 OK, 404 Not Found)
- `GET /api/leads` - List leads with pagination (200 OK)
- `GET /health` - Liveness check (200 OK)
- `GET /health/ready` - Readiness check (200 OK, 503 Service Unavailable)
- `POST /api/webhooks/sales-agent` - Receive conversion feedback (200 OK)

#### HTTP Status Codes
- `200 OK` - Successful GET/POST (non-creation)
- `201 Created` - Successful resource creation
- `204 No Content` - Successful DELETE
- `400 Bad Request` - Validation errors
- `404 Not Found` - Resource not found
- `409 Conflict` - Business rule violation (e.g., duplicate)
- `500 Internal Server Error` - Unexpected server errors
- `503 Service Unavailable` - Degraded/unavailable state

#### Standard Error Response Format
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "email": ["Email address is required", "Invalid email format"],
    "phoneNumber": ["Phone number must be UK format"]
  },
  "traceId": "00-abc123-def456-00",
  "timestamp": "2025-12-12T10:58:14Z"
}
```

#### Standard Success Response Format
```json
{
  "data": { ... },
  "metadata": {
    "requestId": "abc123",
    "timestamp": "2025-12-12T10:58:14Z"
  }
}
```

### Middleware Pipeline Order (Program.cs)

```csharp
// 1. Exception handling (first to catch all errors)
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2. Request ID generation (for traceability)
app.UseMiddleware<RequestIdMiddleware>();

// 3. Request logging (after ID generation)
app.UseMiddleware<RequestLoggingMiddleware>();

// 4. CORS (before authentication)
app.UseCors("AllowedOrigins");

// 5. Authentication (if implemented)
// app.UseAuthentication();

// 6. Authorization (after authentication)
// app.UseAuthorization();

// 7. Rate limiting (after auth to rate-limit per user)
app.UseRateLimiter();

// 8. Compression (near the end)
app.UseResponseCompression();

// 9. Endpoint routing
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");
app.MapGroup("/api").MapLeadEndpoints();
```

### Endpoint Definition Pattern

```csharp
public static class LeadEndpoints
{
    public static RouteGroupBuilder MapLeadEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/leads", CreateLead)
            .WithName("CreateLead")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Create a new lead";
                operation.Description = "Captures lead information from various channels";
                return operation;
            })
            .Produces<LeadResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return group;
    }

    private static async Task<Results<Created<LeadResponse>, ValidationProblem>> CreateLead(
        CreateLeadRequest request,
        ILeadService leadService,
        IValidator<CreateLeadRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await leadService.CreateLeadAsync(request);
        
        return result.Match(
            success => TypedResults.Created($"/api/leads/{success.Id}", success),
            error => TypedResults.ValidationProblem(error.Errors)
        );
    }
}
```

### Result Pattern for Error Handling

```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Error Error { get; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Error);
    }
}
```

### Dependency Injection Registration

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Core services
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IScoringEngine, ScoringEngine>();

// Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateLeadRequestValidator>();

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<SalesAgentHealthCheck>("sales-agent");
```

## Dependencies on Other Decisions

- **ADR-0001**: Backend Technology Stack (prerequisite - .NET 9.0 selected)
- **ADR-0003** (Future): Database technology selection
- **ADR-0004** (Future): Authentication and authorization strategy
- **ADR-0005** (Future): Logging and observability implementation

## References

- **Task 002**: [specs/tasks/002-task-backend-api-scaffolding.md](../tasks/002-task-backend-api-scaffolding.md) - API scaffolding requirements
- [Minimal APIs Overview](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/overview)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [ASP.NET Core Middleware](https://learn.microsoft.com/aspnet/core/fundamentals/middleware/)
- [OpenAPI Support in Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/openapi)
