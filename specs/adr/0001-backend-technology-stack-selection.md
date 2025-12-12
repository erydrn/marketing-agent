# ADR 0001: Backend Technology Stack Selection

**Status:** Proposed  
**Date:** 2025-12-12  
**Decision Makers:** Architecture Team  
**Related Tasks:** 002-task-backend-api-scaffolding.md, 003-task-data-persistence-layer.md, 004-task-lead-capture-api.md  
**Related Features:** FRD-001 (Lead Capture & Multi-Channel Integration)

---

## Context and Problem Statement

The Digital Marketing Agent system requires a backend API to handle multi-channel lead capture for a legal services business (conveyancing). The system must capture, validate, and store leads from multiple sources including web forms, ad platforms, partner referrals, developer uploads, and event registrations.

**Key Requirements:**
- Handle 100 requests/second minimum throughput
- Process 50,000 leads/month (~5KB per lead = 250MB/month)
- REST API with OpenAPI/Swagger documentation
- Multi-channel integrations (webhooks, bulk uploads)
- Data validation and quality checks
- GDPR compliance with audit trails
- 99.5% uptime requirement
- Azure cloud deployment target
- Response time P95 < 500ms for single lead capture
- Support for graceful shutdown, health checks, structured logging

**Technical Constraints:**
- Must integrate with Azure services
- Need for automated deployment (IaC with Bicep)
- Must support connection pooling and database transactions
- Require ORM/data access framework for maintainability
- Need migration tooling for schema management

---

## Decision Drivers

### Business Priorities
1. **Time to Market** - Quick scaffolding and developer productivity
2. **Azure Integration** - Native support for Azure services and deployment
3. **Maintainability** - Strong typing, mature ecosystem, long-term support
4. **Team Skills** - Consider available talent pool and learning curve
5. **Cost Efficiency** - Azure hosting costs and operational expenses

### Technical Priorities
1. **Performance** - Handle required throughput with efficient resource usage
2. **Type Safety** - Reduce runtime errors with compile-time checks
3. **Ecosystem Maturity** - Battle-tested libraries for common requirements
4. **Observability** - Built-in support for logging, metrics, tracing
5. **Security** - Strong security practices and vulnerability management

---

## Options Considered

### Option 1: .NET 8 (C#) with ASP.NET Core

**Framework:** ASP.NET Core 8 (LTS until November 2026)  
**Database:** Azure SQL Database or PostgreSQL  
**ORM:** Entity Framework Core 8  
**API Documentation:** Swashbuckle (Swagger/OpenAPI)

**Advantages:**
- ✅ **Native Azure Integration** - First-class support for all Azure services (App Service, Functions, SQL, Key Vault, Service Bus)
- ✅ **Excellent Performance** - Consistently top-tier in TechEmpower benchmarks (handles 100k+ req/sec)
- ✅ **Strong Type Safety** - C# compile-time checking reduces runtime errors
- ✅ **Mature Ecosystem** - Comprehensive libraries for validation (FluentValidation), logging (Serilog), testing (xUnit, NUnit)
- ✅ **Built-in DI Container** - Dependency injection native to ASP.NET Core
- ✅ **OpenAPI Generation** - Automatic Swagger documentation from code annotations
- ✅ **EF Core Migrations** - Robust schema migration tooling with rollback support
- ✅ **Azure Tooling** - Excellent Bicep support, Azure SDK for .NET, Azure Developer CLI integration
- ✅ **Enterprise Proven** - Widely used in financial services and legal tech sectors
- ✅ **Long-term Support** - Microsoft LTS commitment, predictable release cycles

**Disadvantages:**
- ⚠️ **Learning Curve** - Steeper for developers without C# experience
- ⚠️ **Verbosity** - More boilerplate code compared to Python/Node.js
- ⚠️ **Windows Heritage** - Historically Windows-focused (though fully cross-platform now)

**Technology Stack Details:**
```
Runtime: .NET 8 LTS
Framework: ASP.NET Core 8
ORM: Entity Framework Core 8
Database: Azure SQL Database or PostgreSQL on Azure
API Docs: Swashbuckle.AspNetCore (OpenAPI 3.0)
Validation: FluentValidation
Logging: Serilog with structured logging
Testing: xUnit + Moq + FluentAssertions
HTTP Client: HttpClient with Polly (resilience)
Deployment: Azure App Service with Bicep IaC
```

**Performance Characteristics:**
- Request throughput: 100,000+ req/sec (far exceeds 100 req/sec requirement)
- Memory efficiency: ~50-100MB baseline, efficient GC
- Startup time: ~2-3 seconds
- P95 latency: <50ms for simple CRUD operations

**Azure Integration Examples:**
```csharp
// Native Azure Key Vault integration
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());

// Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

// Azure SQL with managed identity
services.AddDbContext<LeadContext>(options =>
    options.UseSqlServer(connectionString, 
        sqlOptions => sqlOptions.EnableRetryOnFailure()));
```

---

### Option 2: Node.js with Express/Fastify + TypeScript

**Framework:** Express.js or Fastify  
**Database:** PostgreSQL on Azure  
**ORM:** Prisma or TypeORM  
**API Documentation:** swagger-jsdoc + swagger-ui-express

**Advantages:**
- ✅ **Rapid Development** - Minimal boilerplate, quick prototyping
- ✅ **JavaScript Ecosystem** - Massive npm package availability
- ✅ **TypeScript Support** - Type safety when using TypeScript
- ✅ **Async/Await** - Natural asynchronous programming model
- ✅ **JSON Native** - Seamless JSON handling for REST APIs
- ✅ **Developer Familiarity** - Large pool of JavaScript/TypeScript developers

**Disadvantages:**
- ⚠️ **Performance Ceiling** - Single-threaded, lower throughput than .NET/Go
- ⚠️ **Azure Integration** - Third-party SDKs, not first-class citizen
- ⚠️ **Type Safety Limitations** - TypeScript is compile-time only, runtime is still JavaScript
- ⚠️ **Dependency Management** - npm ecosystem can have security/stability issues
- ⚠️ **Memory Usage** - Higher memory footprint than .NET
- ⚠️ **ORM Maturity** - Prisma/TypeORM less mature than EF Core

**Technology Stack Details:**
```
Runtime: Node.js 20 LTS
Framework: Fastify 4 (faster than Express)
Language: TypeScript 5
ORM: Prisma
Database: PostgreSQL on Azure
API Docs: @fastify/swagger
Validation: Zod or Joi
Logging: Pino (structured logging)
Testing: Jest + Supertest
Deployment: Azure App Service or Container Apps
```

**Performance Characteristics:**
- Request throughput: ~10,000-20,000 req/sec (meets 100 req/sec requirement)
- Memory efficiency: ~100-200MB baseline
- Startup time: ~1-2 seconds
- P95 latency: ~100-200ms for simple CRUD operations

---

### Option 3: Python with FastAPI

**Framework:** FastAPI  
**Database:** PostgreSQL on Azure  
**ORM:** SQLAlchemy 2.0  
**API Documentation:** Auto-generated via FastAPI

**Advantages:**
- ✅ **Developer Productivity** - Pythonic, readable code
- ✅ **Automatic OpenAPI** - FastAPI generates OpenAPI specs automatically
- ✅ **Type Hints** - Python 3.10+ type hints with Pydantic validation
- ✅ **Async Support** - Built on Starlette (async framework)
- ✅ **Data Science Integration** - Easy integration with ML/AI libraries if needed
- ✅ **Modern Framework** - FastAPI is relatively new but well-designed

**Disadvantages:**
- ⚠️ **Performance** - Slower than .NET and Node.js
- ⚠️ **Azure Integration** - Third-party support, not native
- ⚠️ **Type Safety** - Python type hints are not enforced at runtime
- ⚠️ **Deployment Complexity** - More complex than .NET on Azure
- ⚠️ **Async Ecosystem Maturity** - Many Python libraries are still synchronous
- ⚠️ **Package Management** - Dependency resolution can be problematic

**Technology Stack Details:**
```
Runtime: Python 3.12
Framework: FastAPI 0.104+
ORM: SQLAlchemy 2.0 + Alembic
Database: PostgreSQL on Azure
API Docs: Built-in (FastAPI auto-generation)
Validation: Pydantic v2
Logging: structlog
Testing: pytest + httpx
Deployment: Azure Container Apps or App Service
```

**Performance Characteristics:**
- Request throughput: ~5,000-10,000 req/sec (meets 100 req/sec requirement)
- Memory efficiency: ~150-300MB baseline
- Startup time: ~3-5 seconds
- P95 latency: ~150-300ms for simple CRUD operations

---

## Decision Outcome

**Chosen Option:** **Option 1 - .NET 8 (C#) with ASP.NET Core**

### Rationale

1. **Azure-First Architecture** - The project targets Azure deployment. .NET provides the best integration with Azure services, Azure Developer CLI, and infrastructure-as-code tooling. This reduces deployment complexity and operational overhead.

2. **Performance Headroom** - While all options meet the minimum 100 req/sec requirement, .NET provides 100x+ headroom for future growth. This is critical for a lead capture system where spikes (e.g., successful ad campaign) could dramatically increase traffic.

3. **Enterprise Reliability** - Legal services require high reliability. .NET's strong typing, mature tooling, and extensive testing frameworks reduce the risk of production issues. The compile-time type checking catches errors before deployment.

4. **Maintainability** - Entity Framework Core provides robust migration tooling, LINQ for type-safe queries, and excellent performance. This matters for long-term maintenance of the data layer.

5. **Cost Efficiency** - .NET's superior performance means smaller Azure resources (lower tier App Service plans) can handle the required load, reducing operational costs.

6. **Security Posture** - Microsoft's security team actively maintains the .NET ecosystem, with rapid patching and clear security advisories. This is essential for GDPR-compliant systems handling personal data.

7. **Team Productivity** - The spec2cloud framework provides Azure tooling (Bicep, azd CLI) that works seamlessly with .NET. The automated deployment pipeline will be simpler to implement and maintain.

### Technology Stack Details

#### Backend Framework
- **Runtime:** .NET 8 LTS (support until November 2026)
- **Framework:** ASP.NET Core 8 Minimal APIs
- **Language:** C# 12

#### Database & Data Access
- **Database:** Azure SQL Database (S1 tier for production, Basic for dev/test)
  - Alternative: PostgreSQL Flexible Server on Azure (if open-source preference)
- **ORM:** Entity Framework Core 8
- **Migrations:** EF Core Migrations with version control
- **Connection Pooling:** Built-in ADO.NET connection pooling

#### API & Documentation
- **API Style:** REST with OpenAPI 3.0 specification
- **Documentation:** Swashbuckle.AspNetCore 6.5+
- **Versioning:** URL-based versioning (/api/v1/)
- **Serialization:** System.Text.Json (built-in, high performance)

#### Validation & Error Handling
- **Validation:** FluentValidation 11.9+
- **Error Handling:** ProblemDetails (RFC 7807 standard)
- **Input Sanitization:** Built-in model binding with validation attributes

#### Logging & Monitoring
- **Structured Logging:** Serilog 3.1+ with sinks for Azure Application Insights
- **Metrics:** Built-in ASP.NET Core metrics + Application Insights
- **Tracing:** OpenTelemetry (optional for distributed tracing)
- **Health Checks:** Microsoft.Extensions.Diagnostics.HealthChecks

#### Testing
- **Unit Testing:** xUnit 2.6+
- **Mocking:** Moq 4.20+
- **Assertions:** FluentAssertions 6.12+
- **Integration Testing:** WebApplicationFactory with in-memory database
- **API Testing:** Microsoft.AspNetCore.Mvc.Testing
- **Coverage:** Coverlet for code coverage analysis

#### Security
- **Authentication:** Azure AD B2C or API Key authentication
- **Authorization:** ASP.NET Core Authorization with policies
- **Secrets Management:** Azure Key Vault
- **CORS:** Built-in CORS middleware
- **Rate Limiting:** AspNetCoreRateLimit or built-in .NET 7+ rate limiting

#### Resilience & Performance
- **HTTP Resilience:** Polly 8.2+ (retry, circuit breaker, timeout policies)
- **Caching:** IMemoryCache for in-memory caching, Redis for distributed caching
- **Compression:** Response compression middleware (gzip, brotli)

#### Deployment & DevOps
- **IaC:** Bicep templates for Azure resources
- **CI/CD:** GitHub Actions
- **Deployment Target:** Azure App Service (Windows or Linux)
- **Container Support:** Docker (optional, for Container Apps)
- **CLI:** Azure Developer CLI (azd) for deployment automation

### Implementation Patterns

#### Repository Pattern
```csharp
public interface ILeadRepository
{
    Task<Lead> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Lead?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Lead>> GetAllAsync(LeadFilter filter, CancellationToken cancellationToken = default);
    Task<Lead> CreateAsync(Lead lead, CancellationToken cancellationToken = default);
    Task UpdateAsync(Lead lead, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
```

#### Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    ILeadRepository Leads { get; }
    ILeadScoreRepository LeadScores { get; }
    ILeadSourceAttributionRepository SourceAttributions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
```

#### Service Layer Pattern
```csharp
public interface ILeadCaptureService
{
    Task<LeadCaptureResult> CaptureWebFormLeadAsync(
        WebFormLeadRequest request, 
        CancellationToken cancellationToken = default);
    
    Task<LeadCaptureResult> CaptureAdPlatformLeadAsync(
        AdPlatformLeadRequest request, 
        CancellationToken cancellationToken = default);
    
    Task<BulkCaptureResult> CaptureBulkLeadsAsync(
        BulkLeadRequest request, 
        CancellationToken cancellationToken = default);
}
```

#### Minimal API Endpoints
```csharp
app.MapPost("/api/v1/leads/web-form", 
    async (WebFormLeadRequest request, ILeadCaptureService service, CancellationToken ct) =>
    {
        var result = await service.CaptureWebFormLeadAsync(request, ct);
        return Results.Created($"/api/v1/leads/{result.LeadId}", result);
    })
    .WithName("CaptureWebFormLead")
    .WithOpenApi()
    .Produces<LeadCaptureResult>(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .RequireRateLimiting("fixed");
```

---

## Consequences

### Positive

1. **Superior Azure Integration** - Seamless deployment, native SDK support, excellent Bicep tooling
2. **Performance Excellence** - Handles 100x+ required throughput with room for growth
3. **Type Safety** - Compile-time error detection reduces production bugs
4. **Robust Tooling** - Mature ecosystem for testing, logging, validation, and monitoring
5. **Enterprise Grade** - Battle-tested in financial and legal sectors with strict compliance requirements
6. **Cost Efficiency** - Smaller Azure resources needed due to superior performance/$ ratio
7. **Long-term Support** - Microsoft LTS commitment provides stability and security patches
8. **Security Posture** - Active security team, rapid CVE patching, built-in security features

### Negative

1. **Learning Curve** - Team members without C# experience will need time to ramp up
2. **Verbosity** - More code required compared to dynamic languages (mitigated by modern C# features)
3. **Ecosystem Bias** - Some open-source tools may have better Node.js/Python support

### Mitigation Strategies

1. **Training** - Provide C#/.NET training resources and pair programming for team onboarding
2. **Code Generation** - Use scaffolding tools (dotnet CLI, Entity Framework CLI) to reduce boilerplate
3. **Best Practices** - Leverage spec2cloud's APM standards packages for .NET best practices
4. **Documentation** - Comprehensive inline documentation and architectural guidelines

---

## Validation

### Performance Benchmarks
- Run TechEmpower benchmarks to validate throughput claims
- Load test with Apache Bench or k6 to ensure 100 req/sec minimum
- Profile memory usage under sustained load

### Proof of Concept
- Build minimal API endpoint with EF Core and Azure SQL
- Test deployment to Azure App Service via Bicep
- Validate OpenAPI documentation generation
- Verify health check and logging integration with Application Insights

### Security Review
- Penetration testing for common OWASP vulnerabilities
- GDPR compliance review for data handling
- Audit trail validation for lead capture events

---

## Related Decisions

- **ADR-002** (Future): Database schema design and indexing strategy
- **ADR-003** (Future): Authentication and authorization approach
- **ADR-004** (Future): Caching strategy (in-memory vs. Redis)
- **ADR-005** (Future): External service integration patterns (webhooks, API clients)

---

## References

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Azure SQL Database Best Practices](https://learn.microsoft.com/en-us/azure/azure-sql/database/performance-guidance)
- [TechEmpower Web Framework Benchmarks](https://www.techempower.com/benchmarks/)
- [Task 002: Backend API Scaffolding](/home/runner/work/marketing-agent/marketing-agent/specs/tasks/002-task-backend-api-scaffolding.md)
- [Task 003: Data Persistence Layer](/home/runner/work/marketing-agent/marketing-agent/specs/tasks/003-task-data-persistence-layer.md)
- [Task 004: Lead Capture API](/home/runner/work/marketing-agent/marketing-agent/specs/tasks/004-task-lead-capture-api.md)
- [FRD-001: Lead Capture Integration](/home/runner/work/marketing-agent/marketing-agent/specs/features/lead-capture-integration.md)
- [.NET 8 Release Notes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Azure App Service for .NET](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore)

---

## Revision History

| Date | Version | Author | Changes |
|------|---------|--------|---------|
| 2025-12-12 | 1.0 | Architecture Team | Initial decision document |
