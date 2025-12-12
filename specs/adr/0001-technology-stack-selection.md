# ADR-0001: Technology Stack Selection

**Date**: 2025-12-12  
**Status**: Proposed

## Context

The Digital Marketing Agent requires a technology stack for implementing the backend API, data persistence layer, and integration capabilities. The project is in the planning phase with the following requirements:

- Multi-channel lead capture from various sources (Google Ads, Facebook, website forms, email)
- Lead scoring and qualification engine
- Integration with Sales Agent and external portals
- Real-time analytics and reporting dashboard
- Azure deployment target
- Support for 100+ leads per minute under load
- GDPR compliance for data handling

The roadmap identifies three potential technology stacks:
1. Node.js with Express
2. .NET with ASP.NET Core
3. Python with FastAPI

**Decision Drivers:**

Based on the PRD and FRD requirements, the key factors influencing this decision are:

- **Performance Requirements**: Process 100 leads/minute with < 5 second capture time
- **Integration Complexity**: Multiple third-party APIs (Google Ads, Facebook, LinkedIn, email parsing)
- **Azure Alignment**: Native Azure SDK support and deployment options
- **Team Expertise**: Availability of development resources (assumed general full-stack capability)
- **Development Speed**: Time to market for MVP (Weeks 1-8)
- **Scalability**: Support for growth from 50K to 500K+ leads per month
- **Type Safety**: Reduce runtime errors in critical lead processing flows
- **Ecosystem Maturity**: Available libraries for data validation, ORM, testing, and integrations

## Decision Drivers

1. **Performance Requirements**
   - Handle 100 concurrent connections
   - Process leads within 5 seconds
   - Support bulk operations for developer lead uploads
   - Real-time dashboard with < 3 second query response

2. **Integration Requirements**
   - Google Ads API (OAuth 2.0)
   - Facebook/Meta Lead Ads API
   - LinkedIn Lead Gen Forms API
   - Email parsing and forwarding
   - Postcode lookup services
   - Sales Agent API integration

3. **Azure Deployment**
   - Must deploy to Azure App Service, Container Apps, or Functions
   - Support Azure SQL Database or Cosmos DB
   - Integration with Application Insights for monitoring
   - Azure AD for authentication (future requirement)

4. **Development Velocity**
   - MVP in 8 weeks (Phase 1-2)
   - Minimal boilerplate and configuration
   - Strong development tooling (IDE support, debugging)
   - Comprehensive testing capabilities

5. **Data Handling**
   - Strong schema validation for lead data
   - ORM/data access framework maturity
   - Database migration tooling
   - Transaction support for data integrity

6. **Compliance & Security**
   - GDPR compliance (data handling, right to erasure)
   - Input validation to prevent injection attacks
   - Secure API authentication
   - Audit logging capabilities

## Considered Options

### Option 1: .NET 8 with ASP.NET Core

**Description**: Use .NET 8 LTS with ASP.NET Core for the backend API, Entity Framework Core for data access, and C# as the primary language.

**Pros**:
- **Strong Type Safety**: C# provides compile-time type checking, reducing runtime errors in critical lead processing logic
- **Excellent Azure Integration**: First-class Azure SDK support, native deployment to Azure App Service/Container Apps
- **Performance**: One of the fastest backend frameworks (comparable to Node.js, faster than Python)
- **Mature Ecosystem**: Entity Framework Core is battle-tested for complex data models with excellent migration support
- **Built-in Validation**: FluentValidation and DataAnnotations for schema validation
- **Enterprise-Grade**: Proven in high-throughput financial and legal applications
- **Long-term Support**: .NET 8 LTS supported until November 2026
- **Comprehensive Testing**: xUnit/NUnit with excellent mocking frameworks (Moq, NSubstitute)
- **Dependency Injection**: Built-in DI container encourages best practices
- **Background Jobs**: Hangfire or Quartz.NET for scheduled tasks (reports, data aggregation)

**Cons**:
- **Steeper Learning Curve**: Requires C# expertise (may require hiring/training if team is primarily JavaScript/Python)
- **Verbosity**: More boilerplate code compared to Python/Node.js for simple endpoints
- **Startup Time**: Slightly slower cold starts compared to Node.js (relevant for Azure Functions)
- **Community Size**: Smaller open-source community compared to Node.js/Python for niche integrations

**Technology Components**:
- **Backend Framework**: ASP.NET Core 8 Web API
- **ORM**: Entity Framework Core 8
- **Database**: Azure SQL Database or PostgreSQL
- **Validation**: FluentValidation
- **Testing**: xUnit, Moq, Testcontainers
- **API Documentation**: Swashbuckle (Swagger/OpenAPI)
- **Background Jobs**: Hangfire
- **Deployment**: Azure App Service or Azure Container Apps

### Option 2: Node.js with Express/Fastify

**Description**: Use Node.js LTS with Express or Fastify framework, Prisma or TypeORM for data access, and TypeScript for type safety.

**Pros**:
- **JavaScript Ecosystem**: Largest package ecosystem (npm) with integrations for every third-party API
- **Fast Development**: Minimal boilerplate, quick prototyping capabilities
- **Non-blocking I/O**: Excellent for I/O-heavy operations (API calls, database queries)
- **TypeScript Support**: Optional type safety while maintaining JavaScript flexibility
- **Developer Familiarity**: Large talent pool with JavaScript expertise
- **Real-time Capabilities**: Native WebSocket support for real-time dashboard
- **JSON Native**: Natural fit for REST APIs and JSON data handling
- **Lightweight**: Small memory footprint, fast startup times

**Cons**:
- **Single-threaded**: Limited CPU-intensive processing (lead scoring algorithms may be bottleneck)
- **Type Safety Optional**: TypeScript can be bypassed, allowing type errors
- **ORM Maturity**: Prisma is newer; TypeORM has stability concerns with complex migrations
- **Error Handling**: Callback/promise chains can become complex without strict patterns
- **Performance Variability**: GC pauses can cause latency spikes under heavy load
- **Azure Integration**: SDK support exists but less mature than .NET

**Technology Components**:
- **Backend Framework**: Express 4.x or Fastify 4.x
- **Language**: TypeScript 5.x
- **ORM**: Prisma 5.x or TypeORM
- **Database**: PostgreSQL or MongoDB
- **Validation**: Zod or Joi
- **Testing**: Jest, Supertest
- **API Documentation**: Swagger-jsdoc
- **Background Jobs**: Bull (Redis-based)
- **Deployment**: Azure App Service (Node.js runtime) or Container Apps

### Option 3: Python with FastAPI

**Description**: Use Python 3.11+ with FastAPI framework, SQLAlchemy for ORM, and Pydantic for data validation.

**Pros**:
- **Rapid Development**: Minimal boilerplate, highly readable code
- **Type Hints**: Native type annotations with runtime validation via Pydantic
- **Excellent Data Processing**: Strong libraries for data analysis (pandas) and ML (scikit-learn) for future lead scoring
- **FastAPI Performance**: One of the fastest Python frameworks (comparable to Node.js)
- **Automatic API Docs**: Built-in Swagger/ReDoc generation from type hints
- **Async Support**: Native async/await for concurrent API calls
- **Developer Productivity**: Concise syntax, powerful standard library
- **AI/ML Future**: Natural fit if predictive lead scoring is added later (Phase 4)

**Cons**:
- **Performance Ceiling**: Slower than .NET/Node.js for CPU-intensive tasks (lead scoring calculations)
- **GIL Limitation**: Global Interpreter Lock limits multi-core parallelism
- **Type Safety**: Type hints are not enforced at runtime (mypy required for strict checking)
- **Azure SDK Maturity**: Less mature than .NET; some services lack Python SDK support
- **Deployment Complexity**: Requires containerization for Azure (less straightforward than .NET App Service)
- **Dependency Management**: Poetry/pip dependency resolution can be fragile
- **Database Migrations**: Alembic (SQLAlchemy migrations) has a steeper learning curve than EF Core

**Technology Components**:
- **Backend Framework**: FastAPI 0.104+
- **Language**: Python 3.11+
- **ORM**: SQLAlchemy 2.x
- **Database**: PostgreSQL
- **Validation**: Pydantic v2
- **Testing**: pytest, httpx
- **API Documentation**: Built-in (FastAPI auto-generates)
- **Background Jobs**: Celery with Redis
- **Deployment**: Azure Container Apps or Azure Functions (Python runtime)

## Decision Outcome

**Chosen Option**: **Option 1: .NET 8 with ASP.NET Core**

**Rationale**:

1. **Azure Native**: The project explicitly targets Azure deployment. .NET provides the most seamless Azure integration with first-class SDK support for Azure SQL Database, Application Insights, Azure AD, and all Azure services. This reduces integration risk and development time.

2. **Type Safety for Critical Operations**: Lead capture and processing are critical business functions. C#'s compile-time type checking prevents entire classes of bugs that could result in lost leads or data corruption. This is more robust than TypeScript (which can be bypassed) or Python type hints (not enforced at runtime).

3. **Data Access Maturity**: Entity Framework Core provides the most mature ORM with excellent migration tooling, transaction support, and query optimization. The repository pattern implementation (required in Task 003) is well-established in the .NET ecosystem.

4. **Performance with Strong Guarantees**: While Node.js and FastAPI can match .NET's throughput, .NET provides more predictable performance under load with better GC tuning and deterministic resource management. This is critical for meeting the < 5 second capture time SLA.

5. **Enterprise Compliance**: Legal services businesses require enterprise-grade security and compliance. .NET's built-in authentication, authorization, and audit logging patterns align with legal industry requirements.

6. **Long-term Maintainability**: The strongly-typed codebase will be easier to refactor as requirements evolve. IntelliSense and compile-time checks reduce maintenance burden.

7. **Alignment with Spec2Cloud Guidelines**: The APM dependencies reference `spec2cloud-guidelines-backend`, which includes .NET best practices. This suggests .NET is the expected choice for this template.

**Trade-offs Accepted**:
- More verbose code compared to Python/Node.js (mitigated by code generation tools and snippets)
- Smaller open-source community for niche integrations (mitigated by commercial library availability and Azure SDK coverage)
- Potential hiring challenge if team lacks C# expertise (mitigated by strong .NET learning resources and similarity to TypeScript/Java)

## Consequences

### Positive

1. **Reduced Integration Risk**: Native Azure SDK support eliminates third-party library dependencies for cloud services
2. **Type Safety**: Compile-time checks prevent lead data corruption and reduce production bugs
3. **Performance Predictability**: Deterministic GC and memory management provide consistent latency
4. **Mature Ecosystem**: Battle-tested libraries for all required functionality (data validation, ORM, background jobs, testing)
5. **Built-in Best Practices**: Dependency injection, configuration management, and logging are first-class framework features
6. **Long-term Support**: .NET 8 LTS provides stability and security updates through 2026
7. **DevOps Integration**: Excellent CI/CD tooling with GitHub Actions, Azure DevOps, and Azure CLI

### Negative

1. **Learning Curve**: Team members unfamiliar with C# will require training (2-4 weeks ramp-up time)
2. **Verbosity**: More lines of code for simple operations compared to Python (offset by tooling and clarity)
3. **Initial Setup Time**: Configuration of Entity Framework, migrations, and dependency injection adds 1-2 days to Task 002
4. **Cold Start Latency**: If using Azure Functions, .NET has slower cold starts than Node.js (not relevant for App Service deployment)

### Neutral

1. **Database Choice Flexibility**: Can use Azure SQL Database (native) or PostgreSQL (cross-platform option)
2. **Testing Strategy**: Will use xUnit with Moq for unit tests and Testcontainers for integration tests
3. **API Documentation**: Swagger/OpenAPI generation via Swashbuckle (standard in all three options)

## Implementation Notes

### Database Selection

- **Primary Recommendation**: **Azure SQL Database**
  - Native .NET integration with SQL Server
  - Excellent Entity Framework Core provider
  - Built-in monitoring with Azure portal
  - Automatic backups and point-in-time restore
  - Scale up/down as needed

- **Alternative**: **PostgreSQL on Azure Database for PostgreSQL**
  - Open-source option for cost sensitivity
  - Excellent EF Core provider (Npgsql)
  - Cross-platform compatibility if non-Azure deployment needed later

### Migration Path

Since this is a greenfield project (no existing codebase), migration concerns are minimal. The implementation tasks are:

1. **Task 001**: Create .NET solution structure with `dotnet new webapi`
2. **Task 002**: Configure Entity Framework Core with Azure SQL Database provider
3. **Task 003**: Implement repository pattern with base repository and unit of work
4. **Task 004+**: Build features incrementally following Phase 1-3 roadmap

### Dependencies on Other Decisions

- **ADR-0002** (Database Schema Design): Will define specific EF Core entity configurations and migration strategy
- **ADR-0003** (Authentication Strategy): Will determine if Azure AD or API Key authentication is used
- **ADR-0004** (Background Job Processing): Will select Hangfire vs Quartz.NET for scheduled reports and data aggregation

### Technology Versions

- **.NET**: 8.0 LTS (supported until November 2026)
- **Entity Framework Core**: 8.0
- **C#**: 12 (latest language features)
- **ASP.NET Core**: 8.0

### Development Environment

- **IDE**: Visual Studio 2022 or Visual Studio Code with C# Dev Kit
- **Database Tools**: Azure Data Studio or SQL Server Management Studio
- **CLI**: .NET CLI (`dotnet`) for build, test, and migration commands
- **DevContainer**: Configured with .NET 8 SDK, Azure CLI, and SQL Server Developer Edition

### Testing Strategy

- **Unit Tests**: xUnit with Moq for mocking database and external services
- **Integration Tests**: Testcontainers for spinning up SQL Server containers
- **API Tests**: WebApplicationFactory for in-memory API testing
- **Performance Tests**: BenchmarkDotNet for load testing critical paths

### Key Libraries

| Purpose | Library | Version |
|---------|---------|---------|
| Web API | ASP.NET Core | 8.0 |
| ORM | Entity Framework Core | 8.0 |
| Database Provider | Microsoft.EntityFrameworkCore.SqlServer | 8.0 |
| Validation | FluentValidation | 11.x |
| Testing | xUnit | 2.6+ |
| Mocking | Moq | 4.20+ |
| API Docs | Swashbuckle.AspNetCore | 6.5+ |
| Background Jobs | Hangfire | 1.8+ |
| Logging | Serilog | 3.1+ |
| HTTP Client | System.Net.Http | Built-in |
| JSON Serialization | System.Text.Json | Built-in |

## References

- [PRD: Digital Marketing Agent](../prd.md) - Requirements REQ-1 through REQ-10
- [FRD-001: Lead Capture Integration](../features/lead-capture-integration.md) - Multi-channel integration requirements
- [FRD-002: Lead Qualification Routing](../features/lead-qualification-routing.md) - Lead scoring engine requirements
- [FRD-003: Analytics Reporting](../features/analytics-reporting.md) - Real-time dashboard requirements
- [Task 003: Data Persistence Layer](../tasks/003-task-data-persistence-layer.md) - ORM and repository pattern requirements
- [Azure Architecture Center: Web API Design](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design)
- [.NET 8 Performance Improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)
