# ADR-0001: Backend Technology Stack Selection

**Date**: 2025-12-12  
**Status**: Proposed

## Context

The Digital Marketing Agent requires a backend API to handle multi-channel lead generation, lead qualification, routing, and integration with external systems. The backend must support:

- **High Availability**: 99.5% uptime requirement (PRD Section 3)
- **Multi-Channel Integration**: Google Ads, Facebook/Meta Ads, LinkedIn, website forms, email parsing
- **External System Integration**: Sales Agent API, Lead System, Digital Quoting, Introducer Portal
- **Azure Deployment**: Target deployment on Azure cloud platform
- **Scalability**: Handle 100+ leads/minute with potential for growth
- **Real-Time Processing**: Lead capture within 5 seconds, routing within 5 seconds
- **Structured Logging**: JSON logs for Application Insights integration
- **API Documentation**: OpenAPI/Swagger specification generation

The devcontainer provides three viable options: .NET 9.0, Python 3.12, and Node.js/TypeScript.

## Decision Drivers

1. **Azure Integration**: Native support for Azure services (App Service, Functions, Container Apps, Application Insights)
2. **Performance**: Ability to handle concurrent requests with low latency
3. **Type Safety**: Strong typing to prevent runtime errors in integration logic
4. **Developer Productivity**: Rich ecosystem, tooling, and debugging support
5. **Enterprise Readiness**: Production-proven for legal/financial services workloads
6. **Team Expertise**: Alignment with available skills (devcontainer suggests .NET focus with C# DevKit)
7. **OpenAPI Support**: First-class support for API documentation generation
8. **Observability**: Built-in telemetry and logging integrations
9. **Long-Term Support**: Microsoft backing and LTS commitment
10. **Testing Infrastructure**: Mature testing frameworks and practices

## Considered Options

### Option 1: .NET 9.0 with ASP.NET Core Minimal APIs

**Description**: Use ASP.NET Core 9.0 with minimal API pattern, C#, Entity Framework Core for data access, and Azure SDK for cloud integrations.

**Pros**:
- ✅ **Exceptional Azure Integration**: First-party support for all Azure services via Azure SDK
- ✅ **.NET Aspire**: Built-in orchestration and observability (already in devcontainer)
- ✅ **Performance**: Industry-leading throughput and low latency (TechEmpower benchmarks)
- ✅ **Type Safety**: Strong typing with C# reduces runtime errors
- ✅ **OpenAPI**: Built-in Swagger/OpenAPI generation via Swashbuckle or NSwag
- ✅ **Middleware Pipeline**: Robust request/response pipeline architecture
- ✅ **Enterprise Ready**: Proven in legal, financial, and healthcare sectors
- ✅ **Dependency Injection**: First-class DI container built-in
- ✅ **Testing**: xUnit, NUnit, MSTest with excellent IDE integration
- ✅ **Long-Term Support**: .NET 9 supported until May 2026; .NET 8 LTS until Nov 2026
- ✅ **Tooling**: VS Code C# DevKit extension already configured in devcontainer
- ✅ **Application Insights**: Native integration for telemetry and monitoring
- ✅ **Health Checks**: Built-in health check middleware and endpoints

**Cons**:
- ⚠️ **Learning Curve**: Steeper for developers unfamiliar with C# and .NET
- ⚠️ **Verbosity**: More boilerplate than Python (mitigated by minimal APIs)
- ⚠️ **Deployment Size**: Larger container images than Node.js (mitigated by .NET 9 improvements)

**Best For**: Enterprise applications requiring high performance, strong typing, and deep Azure integration

---

### Option 2: Python 3.12 with FastAPI

**Description**: Use FastAPI framework with Pydantic for validation, SQLAlchemy for ORM, and Azure SDK for Python for cloud integrations.

**Pros**:
- ✅ **Developer Productivity**: Concise syntax, rapid prototyping
- ✅ **OpenAPI**: Automatic OpenAPI spec generation from type hints
- ✅ **Type Hints**: Gradual typing with Pydantic validation
- ✅ **Azure SDK**: Comprehensive Python SDK for Azure services
- ✅ **Data Science Integration**: Easy integration with pandas, numpy if analytics features expand
- ✅ **Async Support**: Native async/await for concurrent operations
- ✅ **FastAPI**: Modern, high-performance framework with great docs
- ✅ **Testing**: pytest with excellent plugin ecosystem

**Cons**:
- ⚠️ **Performance**: Slower than .NET and Node.js for I/O-bound operations
- ⚠️ **Runtime Errors**: Weaker type safety leads to more runtime errors despite Pydantic
- ⚠️ **GIL Limitations**: Global Interpreter Lock limits true multi-threading
- ⚠️ **Production Tooling**: Less mature observability compared to .NET ecosystem
- ⚠️ **Enterprise Adoption**: Less common in legal/financial enterprise environments
- ⚠️ **Dependency Management**: Poetry/pip ecosystem less robust than NuGet
- ⚠️ **Azure Integration**: SDK coverage good but not as comprehensive as .NET

**Best For**: Data-heavy applications, rapid prototyping, teams with strong Python expertise

---

### Option 3: Node.js with TypeScript and Express/Fastify

**Description**: Use Node.js runtime with TypeScript, Express or Fastify framework, TypeORM/Prisma for data access, and Azure SDK for JavaScript.

**Pros**:
- ✅ **TypeScript**: Strong typing with excellent IDE support
- ✅ **NPM Ecosystem**: Vast package ecosystem
- ✅ **Developer Familiarity**: JavaScript/TypeScript widely known
- ✅ **Async by Default**: Event loop handles concurrent I/O well
- ✅ **Azure SDK**: Comprehensive JavaScript/TypeScript SDK
- ✅ **OpenAPI**: Libraries like tsoa, routing-controllers for spec generation
- ✅ **Testing**: Jest, Mocha, Vitest with good tooling
- ✅ **Lightweight**: Small container images

**Cons**:
- ⚠️ **Type Safety**: TypeScript catches compile-time errors but runtime validation requires additional libraries (zod, joi)
- ⚠️ **Middleware Maturity**: Express aging; Fastify less enterprise-proven than ASP.NET Core
- ⚠️ **Performance**: Good but not as fast as .NET for CPU-intensive operations
- ⚠️ **Enterprise Support**: Node.js LTS but less enterprise tooling than .NET
- ⚠️ **Observability**: Requires more configuration than .NET's built-in Application Insights integration
- ⚠️ **Dependency Management**: npm/yarn security concerns; package.json complexity
- ⚠️ **Production Debugging**: More challenging than .NET in complex scenarios

**Best For**: Teams with strong JavaScript expertise, serverless architectures, real-time applications

---

## Decision Outcome

**Chosen Option**: **.NET 9.0 with ASP.NET Core Minimal APIs**

**Rationale**:

1. **Azure-First Architecture**: The PRD explicitly targets Azure deployment. .NET offers the deepest Azure integration with first-party SDKs, .NET Aspire for orchestration, and native Application Insights support.

2. **Enterprise Readiness**: Legal services require enterprise-grade reliability, security, and compliance. .NET is proven in regulated industries (finance, healthcare, legal).

3. **Performance Requirements**: The 99.5% uptime SLA and sub-5-second processing requirements demand high performance. ASP.NET Core consistently ranks top in TechEmpower benchmarks.

4. **Type Safety for Integrations**: Multiple external integrations (Sales Agent, Google Ads, Lead System, etc.) benefit from C#'s strong typing to catch integration contract errors at compile time.

5. **Devcontainer Alignment**: The devcontainer already includes .NET 9.0, .NET Aspire, and C# DevKit extension, indicating organizational preference.

6. **Observability**: Built-in telemetry, structured logging, and health checks align perfectly with Task 002 requirements (OpenAPI, health endpoints, logging infrastructure).

7. **Long-Term Maintainability**: Strong typing, comprehensive testing tools, and mature middleware patterns reduce technical debt.

8. **Minimal APIs**: .NET 9's minimal API pattern eliminates traditional ASP.NET verbosity while maintaining type safety and performance.

## Consequences

### Positive

- ✅ **Excellent Azure Integration**: First-class support for Application Insights, Key Vault, Service Bus, Storage
- ✅ **High Performance**: Can easily handle 100+ req/sec requirement with room to scale
- ✅ **Type Safety**: Compile-time checking reduces integration bugs
- ✅ **Built-in Features**: Middleware pipeline, DI, health checks, OpenAPI generation out-of-box
- ✅ **Observability**: Native structured logging and telemetry
- ✅ **Enterprise Support**: Microsoft backing with long-term support commitments
- ✅ **Testing Infrastructure**: xUnit/NUnit with excellent test discovery and coverage tools
- ✅ **Security**: Strong security primitives for authentication, authorization, data protection

### Negative

- ⚠️ **Learning Curve**: Team members unfamiliar with C# will need training
  - *Mitigation*: Extensive Microsoft Learn resources, .NET documentation, GitHub Copilot assistance
- ⚠️ **Larger Deployments**: .NET containers larger than Node.js
  - *Mitigation*: .NET 9 improved startup time and image size; use Alpine-based images
- ⚠️ **Compilation Step**: Requires build step unlike interpreted Python
  - *Mitigation*: Fast incremental compilation; hot reload in development

### Neutral

- 📝 **Tooling Setup**: Requires .NET CLI, SDKs (already in devcontainer)
- 📝 **NuGet Packages**: Different package ecosystem than npm/pip
- 📝 **Code Style**: C# conventions differ from Python/JavaScript

## Implementation Notes

### Project Structure

```
src/
├── DigitalMarketingAgent.Api/          # Main API project
│   ├── Program.cs                      # Minimal API entry point
│   ├── appsettings.json                # Configuration
│   ├── Middleware/                     # Custom middleware
│   ├── Endpoints/                      # API endpoint definitions
│   └── DigitalMarketingAgent.Api.csproj
├── DigitalMarketingAgent.Core/         # Business logic
│   ├── Models/                         # Domain models
│   ├── Services/                       # Business services
│   ├── Interfaces/                     # Abstractions
│   └── DigitalMarketingAgent.Core.csproj
├── DigitalMarketingAgent.Infrastructure/ # External integrations
│   ├── Data/                           # Database context
│   ├── Integrations/                   # External API clients
│   └── DigitalMarketingAgent.Infrastructure.csproj
└── DigitalMarketingAgent.Tests/        # Unit/integration tests
    └── DigitalMarketingAgent.Tests.csproj
```

### Key Technologies

- **Framework**: ASP.NET Core 9.0 Minimal APIs
- **ORM**: Entity Framework Core 9.0 (if database needed)
- **Validation**: FluentValidation or built-in Data Annotations
- **OpenAPI**: Swashbuckle.AspNetCore or NSwag
- **Logging**: Microsoft.Extensions.Logging with Serilog
- **Testing**: xUnit with FluentAssertions
- **HTTP Client**: HttpClientFactory for external integrations
- **Observability**: Azure Application Insights SDK
- **Health Checks**: Microsoft.Extensions.Diagnostics.HealthChecks

### Migration Path

Starting from scratch - no migration needed. Project will be initialized using `dotnet new` templates.

### Dependencies on Other Decisions

- **ADR-0002** (Future): Database selection (SQL Server, PostgreSQL, or start with in-memory)
- **ADR-0003** (Future): Authentication strategy (API keys vs OAuth 2.0)
- **ADR-0004** (Future): Deployment approach (App Service, Container Apps, or Functions)

## References

- **PRD**: [specs/prd.md](../prd.md) - System availability requirements (99.5% uptime), Azure deployment
- **Task 002**: [specs/tasks/002-task-backend-api-scaffolding.md](../tasks/002-task-backend-api-scaffolding.md) - Backend API scaffolding requirements
- **Implementation Roadmap**: [specs/tasks/000-IMPLEMENTATION-ROADMAP.md](../tasks/000-IMPLEMENTATION-ROADMAP.md) - Lines 55-74 define this task
- [ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [TechEmpower Benchmarks](https://www.techempower.com/benchmarks/)
- [Azure SDK for .NET](https://azure.github.io/azure-sdk/releases/latest/dotnet.html)
