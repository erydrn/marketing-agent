# Backend Technology Stack - Quick Reference

This document provides a quick reference guide for the recommended backend technology stack based on [ADR-0001: Backend Technology Stack Selection](./adr/0001-backend-technology-stack-selection.md).

## 📋 Executive Summary

**Recommended Stack:** .NET 8 (C#) with ASP.NET Core

**Key Reasons:**
- Native Azure integration and deployment tooling
- Superior performance (100x+ required throughput)
- Enterprise-grade reliability and security
- Strong type safety and compile-time error detection
- Mature ecosystem with robust testing and monitoring tools

---

## 🛠️ Technology Stack Components

### Core Framework
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Runtime | .NET | 8 LTS | Application runtime environment |
| Framework | ASP.NET Core | 8 | Web API framework |
| Language | C# | 12 | Primary programming language |
| API Style | Minimal APIs | - | Lightweight REST endpoint definition |

### Database & Data Access
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Database | Azure SQL Database | - | Primary data store |
| Alternative DB | PostgreSQL Flexible Server | - | Open-source option |
| ORM | Entity Framework Core | 8 | Object-relational mapping |
| Migrations | EF Core Migrations | - | Schema version control |
| Query Language | LINQ | - | Type-safe database queries |

### API & Documentation
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| API Specification | OpenAPI | 3.0 | API contract definition |
| Documentation UI | Swashbuckle.AspNetCore | 6.5+ | Interactive API documentation |
| Versioning | URL-based | - | API version management (/api/v1/) |
| Serialization | System.Text.Json | - | High-performance JSON handling |

### Validation & Error Handling
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Validation | FluentValidation | 11.9+ | Complex validation rules |
| Error Format | ProblemDetails | RFC 7807 | Standardized error responses |
| Model Binding | Built-in | - | Automatic request parsing |

### Logging & Monitoring
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Structured Logging | Serilog | 3.1+ | Comprehensive logging framework |
| Application Insights | Azure Monitor | - | Performance monitoring & diagnostics |
| Health Checks | Microsoft.Extensions.Diagnostics | - | Service health monitoring |
| Tracing | OpenTelemetry | Optional | Distributed tracing |

### Testing
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Unit Testing | xUnit | 2.6+ | Unit test framework |
| Mocking | Moq | 4.20+ | Mock object creation |
| Assertions | FluentAssertions | 6.12+ | Readable test assertions |
| Integration Testing | WebApplicationFactory | - | Full-stack API testing |
| Code Coverage | Coverlet | - | Coverage analysis |

### Security
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Secrets Management | Azure Key Vault | - | Secure credential storage |
| Authentication | Azure AD B2C / API Keys | - | Identity management |
| Authorization | ASP.NET Core Policies | - | Role/claim-based access |
| Rate Limiting | Built-in / AspNetCoreRateLimit | - | API rate limiting |
| CORS | Built-in middleware | - | Cross-origin resource sharing |

### Resilience & Performance
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Resilience Patterns | Polly | 8.2+ | Retry, circuit breaker, timeout |
| In-Memory Cache | IMemoryCache | - | Local caching |
| Distributed Cache | Azure Cache for Redis | Optional | Distributed caching |
| Compression | Built-in middleware | - | Response compression (gzip, brotli) |

### Deployment & DevOps
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| IaC | Bicep | - | Azure resource provisioning |
| CI/CD | GitHub Actions | - | Automated build and deployment |
| Deployment Target | Azure App Service | - | Managed hosting platform |
| Container Support | Docker | Optional | Containerization |
| CLI Tool | Azure Developer CLI (azd) | - | Deployment automation |

---

## 📦 NuGet Packages (Core Dependencies)

### Essential Packages
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.*" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.*" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.9.*" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.*" />
<PackageReference Include="Serilog.Sinks.ApplicationInsights" Version="4.0.*" />
```

### Testing Packages
```xml
<PackageReference Include="xunit" Version="2.6.*" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.*" />
<PackageReference Include="Moq" Version="4.20.*" />
<PackageReference Include="FluentAssertions" Version="6.12.*" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.*" />
<PackageReference Include="coverlet.collector" Version="6.0.*" />
```

### Azure & Resilience Packages
```xml
<PackageReference Include="Azure.Identity" Version="1.10.*" />
<PackageReference Include="Azure.Security.KeyVault.Secrets" Version="4.5.*" />
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.21.*" />
<PackageReference Include="Polly" Version="8.2.*" />
<PackageReference Include="AspNetCoreRateLimit" Version="5.0.*" />
```

---

## 🚀 Quick Start Commands

### Create New Project
```bash
# Create solution and API project
dotnet new sln -n MarketingAgent
dotnet new webapi -n MarketingAgent.Api -minimal
dotnet sln add MarketingAgent.Api

# Add required packages
cd MarketingAgent.Api
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package FluentValidation.AspNetCore
dotnet add package Serilog.AspNetCore
```

### Database Migrations
```bash
# Install EF Core tools
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName
```

### Run Development Server
```bash
# Run with hot reload
dotnet watch run

# Run with specific environment
dotnet run --environment Development

# Run tests
dotnet test --collect:"XPlat Code Coverage"
```

### Azure Deployment
```bash
# Initialize Azure Developer CLI
azd init

# Provision Azure resources
azd provision

# Deploy application
azd deploy

# Monitor application
azd monitor
```

---

## 📊 Performance Characteristics

| Metric | Expected Value | Requirement | Status |
|--------|---------------|-------------|--------|
| Request Throughput | 100,000+ req/sec | 100 req/sec | ✅ Exceeds 1000x |
| P95 Latency | <50ms | <500ms | ✅ 10x better |
| Memory (Baseline) | 50-100MB | N/A | ✅ Efficient |
| Startup Time | 2-3 seconds | N/A | ✅ Fast |
| Database Query (Simple) | <10ms | <100ms | ✅ 10x better |

---

## 🔐 Security Features

- **Compile-time Type Safety** - Catches errors before runtime
- **Built-in CORS Middleware** - Secure cross-origin requests
- **Azure Key Vault Integration** - Centralized secret management
- **Rate Limiting** - Protection against abuse
- **Input Validation** - FluentValidation for complex rules
- **SQL Injection Protection** - Parameterized queries via EF Core
- **HTTPS Enforcement** - Secure communication by default
- **Security Headers** - X-Frame-Options, CSP, etc.

---

## 📚 Learning Resources

### Official Documentation
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Azure SQL Database](https://learn.microsoft.com/en-us/azure/azure-sql/database/)
- [Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/)

### Tutorials
- [Build a web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)
- [EF Core Getting Started](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app)
- [Deploy ASP.NET Core to Azure](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore)

### Best Practices
- [ASP.NET Core Performance Best Practices](https://learn.microsoft.com/en-us/aspnet/core/performance/performance-best-practices)
- [EF Core Performance](https://learn.microsoft.com/en-us/ef/core/performance/)
- [Azure Architecture Center](https://learn.microsoft.com/en-us/azure/architecture/)

---

## 🎯 Next Steps

1. **Review ADR-0001** - Read the full architecture decision record for detailed rationale
2. **Setup Development Environment** - Install .NET 8 SDK and required tools
3. **Create Project Structure** - Use dotnet CLI to scaffold the initial project
4. **Implement Task 002** - Backend API scaffolding (middleware, logging, health checks)
5. **Implement Task 003** - Data persistence layer (EF Core, repositories, migrations)
6. **Implement Task 004** - Lead capture API endpoints
7. **Deploy to Azure** - Use Bicep and Azure Developer CLI for deployment

---

## ❓ FAQ

**Q: Why not Node.js/TypeScript?**  
A: While Node.js is excellent for many use cases, .NET offers superior Azure integration, better performance, and stronger type safety for this enterprise project.

**Q: Can I use PostgreSQL instead of Azure SQL?**  
A: Yes! Simply change the Entity Framework provider to `Npgsql.EntityFrameworkCore.PostgreSQL`. The rest of the stack remains the same.

**Q: What about Python/FastAPI?**  
A: FastAPI is a great framework, but .NET provides better performance, Azure integration, and enterprise tooling for this project's requirements.

**Q: Is this overkill for a simple API?**  
A: The stack provides excellent scaffolding tools that reduce boilerplate. The investment pays off in maintainability, performance, and Azure integration.

**Q: What if the team doesn't know C#?**  
A: C# has excellent learning resources, and modern C# (8-12) is highly productive. The structured typing reduces bugs and makes the codebase easier to understand.

---

For detailed technical analysis and decision rationale, see [ADR-0001: Backend Technology Stack Selection](./adr/0001-backend-technology-stack-selection.md).
