# Marketing Agent - Source Code

## Project Structure

This solution follows Clean Architecture principles with clear separation of concerns:

### Projects

#### `MarketingAgent.Core`
Domain layer containing business entities, interfaces, and core business logic.
- **Entities**: Domain models (Lead, LeadScore, LeadSourceAttribution)
- **Interfaces**: Repository and service contracts
- **Enums**: Business enumerations
- **Exceptions**: Custom domain exceptions
- **No external dependencies** (framework-agnostic)

#### `MarketingAgent.Infrastructure`
Infrastructure layer handling data persistence, external integrations, and technical concerns.
- **Data Access**: Entity Framework Core DbContext and configurations
- **Repositories**: Implementation of repository interfaces
- **Migrations**: Database migrations
- **External Services**: Third-party API integrations
- **Depends on**: MarketingAgent.Core

#### `MarketingAgent.Api`
Presentation layer exposing REST API endpoints.
- **Controllers**: API endpoints
- **DTOs**: Data transfer objects
- **Middleware**: Request/response pipeline
- **Configuration**: Dependency injection setup
- **Depends on**: MarketingAgent.Core, MarketingAgent.Infrastructure

## Technology Stack

- **.NET**: 8.0 (LTS)
- **Language**: C# 12
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **Database**: Azure SQL Database
- **Testing**: xUnit, Moq, Testcontainers
- **API Docs**: Swagger/OpenAPI (Swashbuckle)

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Azure SQL Database (or SQL Server for local development)
- Visual Studio 2022, Visual Studio Code, or JetBrains Rider

### Building the Solution

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Run tests
dotnet test

# Run the API
cd src/MarketingAgent.Api
dotnet run
```

### Configuration

The API uses environment-based configuration. See `appsettings.json` and `appsettings.Development.json` for settings.

Required configuration:
- **ConnectionStrings:DefaultConnection** - Database connection string
- **Logging** - Logging levels and providers

## Architecture Principles

1. **Dependency Inversion**: Core layer has no dependencies on infrastructure
2. **Repository Pattern**: Data access abstracted through repositories
3. **Unit of Work**: Transaction management
4. **Clean Architecture**: Clear boundaries between layers
5. **SOLID Principles**: Applied throughout

## References

- [Task 001: Project Scaffolding](../../specs/tasks/001-task-project-scaffolding.md)
- [Task 002: Backend API Scaffolding](../../specs/tasks/002-task-backend-api-scaffolding.md)
- [Task 003: Data Persistence Layer](../../specs/tasks/003-task-data-persistence-layer.md)
- [ADR-0001: Technology Stack Selection](../../specs/adr/0001-technology-stack-selection.md)
- [ADR-0002: Database and ORM Strategy](../../specs/adr/0002-database-and-orm-strategy.md)
