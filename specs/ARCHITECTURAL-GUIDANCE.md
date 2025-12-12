# Architectural Guidance for Task 003: Data Storage & Persistence Layer

**Date**: 2025-12-12  
**Architect**: Architect Agent  
**Status**: Guidance Provided

---

## Executive Summary

This document provides comprehensive guidance for implementing **Task 003: Data Storage & Persistence Layer Setup** for the Digital Marketing Agent project. It addresses your questions about technology stack selection, dependency management, and implementation approach.

---

## Answers to Your Questions

### 1. What technology stack should I use for this implementation?

**Answer**: **Use .NET 8 with ASP.NET Core and Azure SQL Database**

This decision is documented in **ADR-0001: Technology Stack Selection** and **ADR-0002: Database and ORM Strategy**.

**Rationale**:
- ✅ **Azure-Native**: First-class Azure integration with Azure SQL Database, Azure App Service, and Application Insights
- ✅ **Type Safety**: C# compile-time type checking prevents lead data corruption
- ✅ **Mature Ecosystem**: Entity Framework Core provides battle-tested ORM with excellent migration tooling
- ✅ **Performance**: Meets requirements for 100 leads/minute with < 5 second capture time
- ✅ **Enterprise-Grade**: Proven in legal and financial industries (compliance requirements)
- ✅ **Long-term Support**: .NET 8 LTS supported until November 2026

**Technology Stack Summary**:

| Component | Technology | Version |
|-----------|-----------|---------|
| **Backend Framework** | ASP.NET Core | 8.0 |
| **Language** | C# | 12 |
| **ORM** | Entity Framework Core | 8.0 |
| **Database** | Azure SQL Database | Latest |
| **Database Provider** | Microsoft.EntityFrameworkCore.SqlServer | 8.0 |
| **Validation** | FluentValidation | 11.x |
| **Testing** | xUnit, Moq, Testcontainers | Latest |
| **API Documentation** | Swashbuckle (Swagger/OpenAPI) | 6.5+ |
| **Background Jobs** | Hangfire | 1.8+ |
| **Logging** | Serilog | 3.1+ |

---

### 2. Should I implement Tasks 001 and 002 first, or can I proceed with Task 003?

**Answer**: **You MUST implement Tasks 001 and 002 before starting Task 003.**

Task dependencies from the Implementation Roadmap:

```
Task 001: Project Scaffolding (P0 - Blocker)
  └─→ Task 002: Backend API Scaffolding (P0 - Blocker)
       └─→ Task 003: Data Persistence Layer (P0 - Critical)
```

**Why This Order Matters**:

1. **Task 001: Project Scaffolding** (Estimated: 2 days)
   - Creates the .NET solution structure (`dotnet new sln`, `dotnet new webapi`)
   - Sets up project folders (src/, tests/, specs/, docs/)
   - Configures build tools (MSBuild, .csproj files)
   - Establishes CI/CD pipeline skeleton (GitHub Actions for .NET)
   - Configures linting and code quality tools (StyleCop, SonarAnalyzer)
   
   **Without this**: You have no project to add Entity Framework Core to.

2. **Task 002: Backend API Scaffolding** (Estimated: 2 days)
   - Scaffolds ASP.NET Core Web API project structure
   - Configures dependency injection container
   - Sets up configuration management (appsettings.json, environment variables)
   - Implements health check endpoints
   - Configures logging and error handling middleware
   - Sets up Swagger/OpenAPI documentation
   
   **Without this**: You cannot register DbContext with dependency injection or configure connection strings.

3. **Task 003: Data Persistence Layer** (Estimated: 3 days)
   - Installs Entity Framework Core packages
   - Configures DbContext with Azure SQL Database provider
   - Defines entity models (Lead, LeadScore, LeadSourceAttribution)
   - Creates initial migration
   - Implements repository pattern and unit of work
   - Configures connection pooling and retry policies

**Recommendation**: Complete Tasks 001 and 002 sequentially before starting Task 003.

---

### 3. What are the recommended patterns and guidelines from the APM modules?

**Answer**: The APM modules are defined in `apm.yml` but **not yet installed**.

**Current APM Configuration** (`apm.yml`):
```yaml
dependencies:
  apm:
    - EmeaAppGbb/spec2cloud-guidelines
    - EmeaAppGbb/spec2cloud-guidelines-backend
    - EmeaAppGbb/spec2cloud-guidelines-frontend
```

**Action Required**: Install APM modules to access guidelines.

```bash
# Install APM dependencies
apm install

# Compile guidelines into AGENTS.md
apm compile
```

**Expected Guidelines** (based on spec2cloud-guidelines-backend for .NET):

1. **Repository Pattern**:
   - Use generic repository interfaces for CRUD operations
   - Implement Unit of Work pattern for transaction management
   - Avoid leaking EF Core types into business logic layer

2. **Entity Configuration**:
   - Use Fluent API in `OnModelCreating()` for entity configuration (prefer over data annotations)
   - Define indexes, constraints, and relationships explicitly
   - Use value objects for complex types (e.g., Address, PhoneNumber)

3. **Migration Management**:
   - Never modify existing migrations (create new ones instead)
   - Review generated SQL scripts before applying to production
   - Use separate migration scripts for data seeding vs. schema changes

4. **Error Handling**:
   - Handle DbUpdateConcurrencyException for optimistic concurrency conflicts
   - Implement retry policies for transient SQL errors
   - Log EF Core query performance warnings

5. **Testing**:
   - Use in-memory database for unit tests
   - Use Testcontainers for integration tests with real SQL Server
   - Mock IRepository interfaces in service layer tests

6. **Performance**:
   - Use `.AsNoTracking()` for read-only queries
   - Use `.Include()` for eager loading (avoid N+1 queries)
   - Use pagination for large result sets (never return unbounded queries)

**Note**: These are expected patterns. Run `apm install && apm compile` to get the full, authoritative guidelines in `AGENTS.md`.

---

## Implementation Roadmap for Task 003

Based on the ADRs and task requirements, here's the recommended implementation sequence:

### Phase 1: Prerequisites (Tasks 001-002)

**Task 001: Project Scaffolding** (Day 1-2)
- Create .NET solution: `dotnet new sln -n MarketingAgent`
- Create Web API project: `dotnet new webapi -n MarketingAgent.Api`
- Create Class Library for models: `dotnet new classlib -n MarketingAgent.Core`
- Create Test project: `dotnet new xunit -n MarketingAgent.Tests`
- Add projects to solution: `dotnet sln add MarketingAgent.Api MarketingAgent.Core MarketingAgent.Tests`
- Configure `.gitignore`, `.editorconfig`, `Directory.Build.props`
- Set up GitHub Actions workflow for .NET build and test

**Task 002: Backend API Scaffolding** (Day 3-4)
- Configure `Program.cs` with minimal API or controllers
- Set up dependency injection for services
- Configure logging with Serilog
- Add health check endpoints (`/health`, `/health/ready`)
- Configure CORS for frontend integration
- Set up Swagger/OpenAPI with Swashbuckle
- Add environment-based configuration (Development, Staging, Production)
- Test: API runs and returns 200 OK on health endpoint

### Phase 2: Entity Framework Core Setup (Task 003 - Days 1-2)

**Day 1: EF Core Installation and Configuration**

1. **Install NuGet Packages** (in `MarketingAgent.Api.csproj`):
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

2. **Create DbContext** (`Data/MarketingAgentDbContext.cs`):
   ```csharp
   public class MarketingAgentDbContext : DbContext
   {
       public MarketingAgentDbContext(DbContextOptions<MarketingAgentDbContext> options)
           : base(options) { }
       
       public DbSet<Lead> Leads => Set<Lead>();
       public DbSet<LeadScore> LeadScores => Set<LeadScore>();
       public DbSet<LeadSourceAttribution> LeadSourceAttributions => Set<LeadSourceAttribution>();
       
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarketingAgentDbContext).Assembly);
       }
   }
   ```

3. **Register DbContext in Program.cs**:
   ```csharp
   builder.Services.AddDbContext<MarketingAgentDbContext>(options =>
       options.UseSqlServer(
           builder.Configuration.GetConnectionString("DefaultConnection"),
           sqlOptions =>
           {
               sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30));
               sqlOptions.CommandTimeout(60);
           }));
   ```

4. **Configure Connection String** (`appsettings.Development.json`):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MarketingAgentDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

**Day 2: Define Entity Models**

5. **Create Entity Classes** (`Models/` folder):

   **Lead.cs**:
   ```csharp
   public class Lead
   {
       public Guid Id { get; set; }
       public string ExternalLeadId { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; }
       public DateTime UpdatedAt { get; set; }
       public DateTime? DeletedAt { get; set; }
       [Timestamp]
       public byte[] RowVersion { get; set; } = Array.Empty<byte>();
       
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string Email { get; set; } = string.Empty;
       public string Phone { get; set; } = string.Empty;
       public string Address { get; set; } = string.Empty;
       public string Postcode { get; set; } = string.Empty;
       
       public string PropertyType { get; set; } = string.Empty;
       public string ServiceType { get; set; } = string.Empty;
       public string Timeline { get; set; } = string.Empty;
       public string Urgency { get; set; } = string.Empty;
       
       // Navigation properties
       public ICollection<LeadScore> LeadScores { get; set; } = new List<LeadScore>();
       public ICollection<LeadSourceAttribution> SourceAttributions { get; set; } = new List<LeadSourceAttribution>();
   }
   ```

   **LeadScore.cs**:
   ```csharp
   public class LeadScore
   {
       public Guid Id { get; set; }
       public Guid LeadId { get; set; }
       public int OverallScore { get; set; }
       public string Tier { get; set; } = string.Empty; // Hot/Warm/Cool/Cold
       public int CompletenessScore { get; set; }
       public int EngagementScore { get; set; }
       public int ReadinessScore { get; set; }
       public int SourceQualityScore { get; set; }
       public DateTime CalculatedAt { get; set; }
       
       // Navigation property
       public Lead Lead { get; set; } = null!;
   }
   ```

   **LeadSourceAttribution.cs**:
   ```csharp
   public class LeadSourceAttribution
   {
       public Guid Id { get; set; }
       public Guid LeadId { get; set; }
       public string Channel { get; set; } = string.Empty;
       public string Source { get; set; } = string.Empty;
       public string Campaign { get; set; } = string.Empty;
       public string Medium { get; set; } = string.Empty;
       public string UtmSource { get; set; } = string.Empty;
       public string UtmMedium { get; set; } = string.Empty;
       public string UtmCampaign { get; set; } = string.Empty;
       public string UtmContent { get; set; } = string.Empty;
       public string UtmTerm { get; set; } = string.Empty;
       public string Referrer { get; set; } = string.Empty;
       public string LandingPage { get; set; } = string.Empty;
       public DateTime CapturedAt { get; set; }
       
       // Navigation property
       public Lead Lead { get; set; } = null!;
   }
   ```

6. **Create Entity Configurations** (`Data/Configurations/` folder):

   **LeadConfiguration.cs**:
   ```csharp
   public class LeadConfiguration : IEntityTypeConfiguration<Lead>
   {
       public void Configure(EntityTypeBuilder<Lead> builder)
       {
           builder.HasKey(l => l.Id);
           builder.Property(l => l.ExternalLeadId).IsRequired().HasMaxLength(100);
           builder.HasIndex(l => l.ExternalLeadId).IsUnique();
           builder.HasIndex(l => l.Email);
           builder.HasIndex(l => l.Postcode);
           builder.HasIndex(l => new { l.CreatedAt, l.PropertyType });
           
           builder.Property(l => l.Email).IsRequired().HasMaxLength(255);
           builder.Property(l => l.Phone).HasMaxLength(20);
           builder.Property(l => l.Postcode).HasMaxLength(10);
           
           // Soft delete query filter
           builder.HasQueryFilter(l => l.DeletedAt == null);
           
           // Temporal table (audit trail)
           builder.ToTable(tb => tb.IsTemporal(temporal =>
           {
               temporal.HasPeriodStart("ValidFrom");
               temporal.HasPeriodEnd("ValidTo");
               temporal.UseHistoryTable("LeadsHistory");
           }));
       }
   }
   ```

### Phase 3: Migrations and Repository Pattern (Task 003 - Day 3)

7. **Create Initial Migration**:
   ```bash
   dotnet ef migrations add InitialCreate --project MarketingAgent.Api
   dotnet ef database update --project MarketingAgent.Api
   ```

8. **Implement Repository Pattern** (`Data/Repositories/` folder):

   **IRepository.cs**:
   ```csharp
   public interface IRepository<T> where T : class
   {
       Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
       Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
       Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
       Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
       Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
       Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
   }
   ```

   **Repository.cs** (generic implementation):
   ```csharp
   public class Repository<T> : IRepository<T> where T : class
   {
       private readonly MarketingAgentDbContext _context;
       private readonly DbSet<T> _dbSet;
       
       public Repository(MarketingAgentDbContext context)
       {
           _context = context;
           _dbSet = context.Set<T>();
       }
       
       public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
       {
           return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
       }
       
       // Implement other methods...
   }
   ```

   **IUnitOfWork.cs**:
   ```csharp
   public interface IUnitOfWork : IDisposable
   {
       IRepository<Lead> Leads { get; }
       IRepository<LeadScore> LeadScores { get; }
       IRepository<LeadSourceAttribution> LeadSourceAttributions { get; }
       Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
       Task BeginTransactionAsync(CancellationToken cancellationToken = default);
       Task CommitTransactionAsync(CancellationToken cancellationToken = default);
       Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
   }
   ```

9. **Register Repositories in Program.cs**:
   ```csharp
   builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
   builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
   ```

### Phase 4: Testing (Task 003 - Day 3)

10. **Unit Tests** (using in-memory database):
    ```csharp
    [Fact]
    public async Task AddLead_ShouldSaveToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MarketingAgentDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        
        using var context = new MarketingAgentDbContext(options);
        var repository = new Repository<Lead>(context);
        
        // Act
        var lead = new Lead { Email = "test@example.com" };
        await repository.AddAsync(lead);
        await context.SaveChangesAsync();
        
        // Assert
        var savedLead = await repository.GetByIdAsync(lead.Id);
        Assert.NotNull(savedLead);
    }
    ```

11. **Integration Tests** (using Testcontainers):
    ```bash
    dotnet add package Testcontainers.MsSql
    ```

---

## Key Deliverables for Task 003

By the end of Task 003, you should have:

- ✅ **DbContext configured** with Azure SQL Database provider
- ✅ **Entity models defined** for Lead, LeadScore, LeadSourceAttribution
- ✅ **Entity configurations** with indexes, constraints, and temporal tables
- ✅ **Initial migration** created and applied to local database
- ✅ **Repository pattern implemented** with generic IRepository<T> and Unit of Work
- ✅ **Dependency injection configured** for DbContext and repositories
- ✅ **Unit tests** for repository CRUD operations (≥85% coverage)
- ✅ **Integration tests** with Testcontainers for database connectivity
- ✅ **Health check** integrated with database connectivity check
- ✅ **Connection pooling and retry policies** configured

---

## Next Steps After Task 003

Once Task 003 is complete:

1. **Task 004: Lead Capture API** - Build POST /api/leads endpoint using the repository pattern
2. **Task 005: Website Form Integration** - Capture leads from web forms
3. **Continue Phase 1 implementation** following the roadmap in `specs/tasks/000-IMPLEMENTATION-ROADMAP.md`

---

## Important Notes

### Database Environment Setup

**Development**:
- Use SQL Server LocalDB (Windows) or Docker container (Mac/Linux)
- Connection string in `appsettings.Development.json`

**Staging/Production**:
- Use Azure SQL Database (provision via Azure Portal or Bicep)
- Store connection string in Azure Key Vault
- Use Managed Identity for authentication (no passwords in config)

### Migration Best Practices

- ✅ **Always review** generated migrations before applying
- ✅ **Test migrations** on a copy of production data before deploying
- ✅ **Never edit** existing migrations (create new ones instead)
- ✅ **Keep rollback scripts** for emergency recovery
- ✅ **Use transactions** for migrations with data changes

### Performance Considerations

- ✅ **Use indexes** on foreign keys, email, postcode, and date fields
- ✅ **Use `.AsNoTracking()`** for read-only queries (30-50% faster)
- ✅ **Implement pagination** for list endpoints (prevent unbounded queries)
- ✅ **Monitor slow queries** with Application Insights and Azure SQL Insights
- ✅ **Use connection pooling** (default in ADO.NET, no extra config needed)

---

## References

- **[ADR-0001: Technology Stack Selection](./adr/0001-technology-stack-selection.md)** - .NET 8 decision rationale
- **[ADR-0002: Database and ORM Strategy](./adr/0002-database-and-orm-strategy.md)** - Azure SQL Database decision rationale
- **[Task 003: Data Persistence Layer](./tasks/003-task-data-persistence-layer.md)** - Full task requirements
- **[Implementation Roadmap](./tasks/000-IMPLEMENTATION-ROADMAP.md)** - Complete project roadmap
- **[Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)** - Official Microsoft docs
- **[Azure SQL Database Best Practices](https://learn.microsoft.com/en-us/azure/azure-sql/database/best-practices)** - Azure guidance

---

## Summary

**You are now ready to implement Task 003** with the following decisions:

✅ **Technology Stack**: .NET 8 + ASP.NET Core + Entity Framework Core + Azure SQL Database  
✅ **Prerequisites**: Must complete Tasks 001 and 002 first (project scaffolding and API setup)  
✅ **Guidelines**: Install APM modules with `apm install && apm compile` to get full `AGENTS.md`  
✅ **Implementation Path**: Follow the phased approach outlined above (EF Core setup → Entity models → Migrations → Repository pattern → Testing)  
✅ **Estimated Timeline**: 3 days for Task 003 after completing Tasks 001-002 (4 days total)

All architectural decisions are documented in ADR-0001 and ADR-0002 for future reference and team alignment.

---

**Document Owner**: Architect Agent  
**Date**: 2025-12-12  
**Status**: Approved for Implementation
