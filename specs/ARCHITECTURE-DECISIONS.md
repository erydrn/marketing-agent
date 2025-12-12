# Technology Stack and Architecture Decision Summary

**Date**: 2025-12-12  
**Status**: Ready for Implementation  
**Task**: 002 - Backend API Scaffolding

## Quick Reference

### Technology Stack Decision
- **Framework**: .NET 9.0 with ASP.NET Core Minimal APIs
- **Architecture**: Clean Architecture (Core/Infrastructure/API layers)
- **ORM**: Entity Framework Core 9.0 (when database is added)
- **Validation**: FluentValidation
- **OpenAPI**: Swashbuckle.AspNetCore
- **Testing**: xUnit with FluentAssertions
- **Logging**: Serilog with Application Insights

### Key Decisions
1. **ADR-0001**: Backend Technology Stack → .NET 9.0
2. **ADR-0002**: API Architecture → Minimal APIs with Clean Architecture

## Project Structure

```
src/
├── DigitalMarketingAgent.Api/          # API Layer (ASP.NET Core)
├── DigitalMarketingAgent.Core/         # Business Logic (framework-agnostic)
├── DigitalMarketingAgent.Infrastructure/  # Data & External Integrations
└── DigitalMarketingAgent.Tests/        # Unit & Integration Tests
```

## API Endpoints (Phase 1)

### Health Checks
- `GET /health` - Liveness probe
- `GET /health/ready` - Readiness probe with dependency checks

### Lead Management (Future)
- `POST /api/leads` - Create lead
- `GET /api/leads/{id}` - Retrieve lead
- `GET /api/leads` - List leads (paginated)

### Documentation
- `GET /swagger` - Swagger UI
- `GET /swagger/v1/swagger.json` - OpenAPI specification

## Core Middleware Stack

1. **Error Handling** - Global exception handler
2. **Request ID** - Generate unique ID for traceability
3. **Request Logging** - Structured logging of all requests
4. **CORS** - Cross-origin resource sharing
5. **Rate Limiting** - Configurable rate limits
6. **Response Compression** - Gzip compression

## Standard Response Formats

### Success Response
```json
{
  "data": { ... },
  "metadata": {
    "requestId": "abc123",
    "timestamp": "2025-12-12T10:58:14Z"
  }
}
```

### Error Response
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "email": ["Email address is required"]
  },
  "traceId": "00-abc123-def456-00",
  "timestamp": "2025-12-12T10:58:14Z"
}
```

## HTTP Status Codes

- `200 OK` - Successful request
- `201 Created` - Resource created successfully
- `400 Bad Request` - Validation error
- `404 Not Found` - Resource not found
- `409 Conflict` - Business rule violation
- `500 Internal Server Error` - Unexpected error
- `503 Service Unavailable` - System degraded/unavailable

## Why .NET 9.0?

1. ✅ **Best Azure Integration** - First-party Azure SDK, Application Insights
2. ✅ **High Performance** - Top TechEmpower benchmark scores
3. ✅ **Type Safety** - Strong typing reduces integration bugs
4. ✅ **Enterprise Ready** - Proven in legal/financial sectors
5. ✅ **Built-in Features** - DI, health checks, OpenAPI out-of-box
6. ✅ **Devcontainer Ready** - .NET 9, Aspire, C# DevKit already configured

## Why Clean Architecture?

1. ✅ **Testability** - Business logic independent of framework
2. ✅ **Maintainability** - Clear separation of concerns
3. ✅ **Flexibility** - Easy to swap infrastructure (database, APIs)
4. ✅ **Domain Focus** - Core models drive the design

## Next Steps

### For Implementation (Dev Agent)
1. Initialize .NET solution with 4 projects
2. Implement middleware pipeline
3. Create health check endpoints
4. Set up OpenAPI/Swagger
5. Configure structured logging
6. Add configuration management
7. Write unit and integration tests

### For Future ADRs
- **ADR-0003**: Database selection (SQL Server, PostgreSQL, or in-memory)
- **ADR-0004**: Authentication strategy (API keys vs OAuth 2.0)
- **ADR-0005**: Logging and observability (Serilog, Application Insights)
- **ADR-0006**: Deployment approach (App Service, Container Apps, Functions)

## Configuration Requirements

### Environment Variables
- `ASPNETCORE_ENVIRONMENT` - Development, Staging, Production
- `APPLICATIONINSIGHTS_CONNECTION_STRING` - Azure Application Insights
- `CORS__ALLOWED_ORIGINS` - Allowed CORS origins
- `RATELIMIT__REQUESTS_PER_MINUTE` - Rate limit configuration

### appsettings.json Structure
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Cors": {
    "AllowedOrigins": ["https://example.com"]
  },
  "RateLimit": {
    "RequestsPerMinute": 100
  }
}
```

## Performance Targets

- **Request Throughput**: 100+ requests/second
- **Response Time**: < 200ms (p95)
- **Health Check**: < 50ms
- **Startup Time**: < 3 seconds
- **Memory Usage**: < 100MB idle

## References

- [ADR-0001: Backend Technology Stack](./adr/0001-backend-technology-stack.md)
- [ADR-0002: API Architecture and Design Patterns](./adr/0002-api-architecture-design-patterns.md)
- [Task 002: Backend API Scaffolding](./tasks/002-task-backend-api-scaffolding.md)
- [ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
