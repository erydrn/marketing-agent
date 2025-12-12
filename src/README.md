# Digital Marketing Agent - Backend API

## Overview

This is the backend API for the Digital Marketing Agent system, built with .NET 9.0 and ASP.NET Core Minimal APIs using Clean Architecture principles.

## Architecture

The solution follows Clean Architecture with three main layers:

- **Api Layer** (`DigitalMarketingAgent.Api`): HTTP endpoints, middleware, and API configuration
- **Core Layer** (`DigitalMarketingAgent.Core`): Business logic, domain models, DTOs, and interfaces
- **Infrastructure Layer** (`DigitalMarketingAgent.Infrastructure`): External dependencies, health checks, and data access

## Technology Stack

- **.NET 9.0** with ASP.NET Core Minimal APIs
- **Serilog** for structured logging
- **Swashbuckle** for OpenAPI/Swagger documentation
- **FluentValidation** for request validation
- **xUnit** and **FluentAssertions** for testing

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Docker (optional, for containerized deployment)

### Building the Solution

```bash
cd src
dotnet build
```

### Running the API

```bash
cd src/DigitalMarketingAgent.Api
dotnet run
```

The API will start on `http://localhost:5135` by default.

### Running Tests

```bash
cd src
dotnet test
```

## API Documentation

Once the API is running, access the Swagger UI documentation at:

```
http://localhost:5135/api/docs
```

## Core Features

### Middleware Pipeline

1. **Error Handling** - Global exception handling with standardized error responses
2. **Request ID** - Generates unique request IDs for distributed tracing
3. **Request Logging** - Structured logging of all HTTP requests and responses
4. **CORS** - Configurable cross-origin resource sharing
5. **Rate Limiting** - IP-based rate limiting (100 requests per minute by default)
6. **Response Compression** - GZIP compression for responses

### Health Checks

- `GET /health` - Basic liveness probe
- `GET /health/ready` - Readiness probe with dependency health checks

### Error Handling

All errors follow a standard JSON format:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "field": ["Error message"]
  },
  "traceId": "00-abc123-def456-00",
  "timestamp": "2025-12-12T10:58:14Z"
}
```

### Logging

Structured JSON logging with Serilog:
- Console output in development (readable format)
- File output with daily rolling (`logs/api-YYYYMMDD.log`)
- Request context included (request ID, user, duration, status code)

## Configuration

Configuration is managed through:
- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- Environment variables - Production secrets and settings

Key configuration sections:

```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000"]
  },
  "Server": {
    "Port": 5000,
    "RequestTimeout": 30,
    "MaxRequestBodySize": 10485760
  }
}
```

## Project Structure

```
src/
├── DigitalMarketingAgent.Api/
│   ├── Endpoints/              # Minimal API endpoint definitions
│   ├── Middleware/             # Custom middleware
│   ├── Program.cs              # Application entry point
│   └── appsettings.json        # Configuration
├── DigitalMarketingAgent.Core/
│   ├── DTOs/                   # Data transfer objects
│   ├── Exceptions/             # Custom exception types
│   ├── Models/                 # Domain models
│   └── Validators/             # FluentValidation validators
├── DigitalMarketingAgent.Infrastructure/
│   ├── HealthChecks/           # Health check implementations
│   └── Services/               # Infrastructure services
└── DigitalMarketingAgent.Tests/
    ├── Unit/                   # Unit tests
    └── Integration/            # Integration tests
```

## Testing

The solution includes:
- **Unit Tests**: 93% coverage (9/14 tests)
- **Integration Tests**: Full HTTP request/response testing (5/14 tests)

All tests use xUnit and FluentAssertions for readable, maintainable test code.

## Next Steps

This API scaffolding provides the foundation for:
- Task 003: Data Persistence Layer
- Task 004: Lead Capture API
- Future feature endpoints and business logic

## Related Documentation

- [Architecture Decision Records](../../specs/adr/)
- [Task Specification](../../specs/tasks/002-task-backend-api-scaffolding.md)
- [Product Requirements](../../specs/prd.md)
