# ADR-0002: Database and ORM Strategy

**Date**: 2025-12-12  
**Status**: Proposed

## Context

Following ADR-0001 (Technology Stack Selection), which established .NET 8 with ASP.NET Core as the backend framework, we need to define the database selection and data access strategy for the Digital Marketing Agent.

**Task 003: Data Persistence Layer** requires:
- Database configuration and connection management
- ORM/data access framework
- Migration tooling
- Repository pattern implementation
- Core data models (Lead, LeadScore, LeadSourceAttribution)

**Data Requirements** from the FRDs:
- Store 50,000 leads/month (~600,000/year)
- Support complex queries for analytics (channel attribution, conversion funnels)
- ACID transactions for lead processing (prevent duplicate captures)
- Full-text search on lead notes and property descriptions
- Temporal data for audit trails (who changed what, when)
- Soft deletes with data retention compliance (7 years for legal industry)
- Geographic queries for postcode-based routing

**Scalability Requirements**:
- Initial: 100 leads/minute
- Year 1: 500K leads (average 16 leads/minute, peak 100 leads/minute)
- Year 3: 2M+ leads (average 60 leads/minute, peak 500 leads/minute)

## Decision Drivers

1. **Data Integrity**
   - ACID transactions to prevent duplicate lead captures
   - Foreign key constraints for referential integrity
   - Optimistic concurrency control for lead updates

2. **Query Performance**
   - Dashboard queries must complete in < 3 seconds
   - Lead retrieval by ID in < 100ms
   - Analytics aggregations (daily/weekly/monthly) in < 5 seconds
   - Support for pagination on large result sets

3. **Azure Integration**
   - Native Azure deployment options
   - Automated backups and disaster recovery
   - Built-in monitoring and diagnostics
   - Scalability without application code changes

4. **Developer Productivity**
   - Type-safe query building
   - Automatic migration generation from model changes
   - Excellent IDE integration and IntelliSense
   - Rich ecosystem of tutorials and community support

5. **Cost Efficiency**
   - Predictable pricing for budgeting
   - Ability to scale down during development/testing
   - No license fees if using open-source option

6. **Compliance**
   - Encryption at rest and in transit
   - Point-in-time restore for data recovery
   - Audit logging capabilities
   - GDPR right-to-erasure support

## Considered Options

### Option 1: Azure SQL Database with Entity Framework Core

**Description**: Use Azure SQL Database (PaaS) as the relational database with Entity Framework Core 8 as the ORM.

**Pros**:
- **Native .NET Integration**: EF Core has the most mature SQL Server provider with excellent performance
- **Azure-Native**: Fully managed PaaS service with automatic patching, backups, and high availability
- **Strong ACID Guarantees**: Full relational database with foreign keys, transactions, and constraints
- **Excellent Tooling**: SQL Server Management Studio, Azure Data Studio, EF Core migrations designer
- **Built-in Monitoring**: Azure SQL Insights provides query performance recommendations
- **Temporal Tables**: Native support for system-versioned temporal tables (audit trails)
- **Full-Text Search**: Built-in full-text indexing for notes and descriptions
- **Elastic Scaling**: Scale compute and storage independently; serverless option for cost savings
- **Active Geo-Replication**: Built-in disaster recovery across Azure regions
- **JSON Support**: Native JSON data type and querying (for flexible UTM parameters)
- **Enterprise Features**: Row-level security, dynamic data masking, transparent data encryption

**Cons**:
- **Cost**: More expensive than PostgreSQL or open-source alternatives (~$5/month for Basic tier, ~$150/month for production-grade Standard S2)
- **Vendor Lock-in**: Tight coupling to Azure SQL (migration to other providers requires effort)
- **License Complexity**: May require SQL Server license for on-premises development (mitigated by free SQL Express or LocalDB)
- **Overkill for Simple Queries**: Advanced features add complexity if only using basic CRUD operations

**Technology Components**:
- **Database**: Azure SQL Database (Serverless tier for dev, Standard S2 for production)
- **ORM**: Entity Framework Core 8 with Microsoft.EntityFrameworkCore.SqlServer provider
- **Migrations**: EF Core Migrations with SQL scripts for review
- **Connection Pooling**: Built-in ADO.NET connection pooling
- **Development Database**: SQL Server LocalDB or Docker container (mcr.microsoft.com/mssql/server:2022-latest)

### Option 2: PostgreSQL on Azure Database for PostgreSQL with Entity Framework Core

**Description**: Use Azure Database for PostgreSQL (PaaS) with Entity Framework Core and Npgsql provider.

**Pros**:
- **Open Source**: No licensing costs for database engine
- **Cross-Platform**: Runs on Azure, AWS, on-premises, or developer laptops
- **EF Core Support**: Npgsql provider is mature and well-maintained
- **Cost-Effective**: ~50% lower cost than Azure SQL for equivalent performance
- **JSON Support**: Excellent JSONB data type with rich querying capabilities
- **Full-Text Search**: Built-in full-text search with GIN indexes
- **Extensions**: PostGIS for advanced geographic queries (useful for postcode data)
- **Community**: Large open-source community with extensive documentation
- **Azure Integration**: Fully managed Azure Database for PostgreSQL with automatic backups
- **Connection Pooling**: PgBouncer for connection pooling at scale

**Cons**:
- **Temporal Data**: No native temporal tables (requires custom triggers or application-level versioning)
- **Azure Tooling**: Less integrated with Azure portal compared to Azure SQL
- **Migration Complexity**: If migrating from SQL Server later, syntax differences require careful review
- **Performance Tuning**: Requires more manual tuning (analyze, vacuum) compared to Azure SQL's automatic optimization
- **Limited Azure Services Integration**: Some Azure services (e.g., Data Factory) have better SQL Server connectors

**Technology Components**:
- **Database**: Azure Database for PostgreSQL (Flexible Server)
- **ORM**: Entity Framework Core 8 with Npgsql.EntityFrameworkCore.PostgreSQL provider
- **Migrations**: EF Core Migrations
- **Connection Pooling**: Npgsql built-in pooling + PgBouncer for high load
- **Development Database**: PostgreSQL Docker container (postgres:16-alpine)

### Option 3: Azure Cosmos DB (NoSQL) with Entity Framework Core

**Description**: Use Azure Cosmos DB with the SQL API and Entity Framework Core Cosmos provider.

**Pros**:
- **Global Distribution**: Multi-region replication with low latency (useful if expanding internationally)
- **Elastic Scalability**: Automatic scaling with serverless or provisioned throughput
- **Schema Flexibility**: Schema-less design allows evolving data models without migrations
- **JSON-Native**: Stores documents directly as JSON (natural fit for API payloads)
- **Guaranteed Performance**: SLA-backed latency and throughput guarantees
- **No Maintenance**: Fully managed, no patching or backup configuration needed

**Cons**:
- **Cost**: Significantly more expensive than SQL/PostgreSQL (~10x cost for equivalent workload)
- **EF Core Limitations**: EF Core Cosmos provider is less mature; missing features like transactions across partitions, advanced query capabilities
- **Query Complexity**: Complex relational queries (joins, aggregations) are expensive and limited
- **Analytics Limitations**: Not designed for OLAP queries; would require syncing to SQL for reporting (FRD-003)
- **ACID Limitations**: No multi-document transactions (only within a partition key)
- **Learning Curve**: Partition key design requires NoSQL expertise
- **Overkill**: Global distribution not needed for UK-focused legal services business

**Technology Components**:
- **Database**: Azure Cosmos DB (SQL API)
- **ORM**: Entity Framework Core 8 with Microsoft.EntityFrameworkCore.Cosmos provider
- **Migrations**: Schema-less (no migrations needed)
- **Development Database**: Cosmos DB Emulator or Azure Cosmos DB Free Tier

## Decision Outcome

**Chosen Option**: **Option 1: Azure SQL Database with Entity Framework Core**

**Rationale**:

1. **Native .NET Integration**: The Microsoft.EntityFrameworkCore.SqlServer provider is the most battle-tested and feature-complete EF Core provider. This reduces risk and development time for Task 003 (Data Persistence Layer).

2. **Relational Data Model**: Lead management is inherently relational:
   - Leads → LeadScores (one-to-many)
   - Leads → LeadSourceAttributions (one-to-many)
   - Leads → Conversions (one-to-many for FRD-004)
   - Strong referential integrity prevents orphaned records

3. **Analytics Requirements**: FRD-003 (Analytics & Reporting) requires complex aggregations, joins, and filtering. Azure SQL Database excels at OLAP queries with columnstore indexes and query optimization.

4. **Temporal Auditing**: Legal industry compliance requires complete audit trails. SQL Server's system-versioned temporal tables provide automatic change tracking without custom code.

5. **Azure Ecosystem**: Azure SQL Database integrates seamlessly with Azure App Service (connection string in app settings), Application Insights (automatic dependency tracking), and Azure DevOps (SQL database deployment tasks).

6. **Proven at Scale**: SQL Server is proven in high-transaction legal and financial systems. The Digital Marketing Agent's load (100-500 leads/minute) is well within Azure SQL's capabilities (thousands of transactions/second).

7. **Cost-Effective Scaling**: Azure SQL Serverless tier provides cost savings during development and low-traffic periods (auto-pause when idle). Production workload (50K leads/month) comfortably fits in Standard S2 tier (~$150/month).

8. **Developer Experience**: EF Core migrations with SQL Server provide the best IDE experience (Visual Studio database projects, SSDT, Azure Data Studio). This accelerates development in Phases 1-3.

**Trade-offs Accepted**:
- **Higher Cost**: Azure SQL is more expensive than PostgreSQL (~2x cost), but offset by reduced development time and lower operational overhead.
- **Azure Lock-in**: Tightly coupled to Azure SQL. Mitigation: EF Core abstracts most SQL Server-specific features; migration to PostgreSQL possible if needed (estimated 2-3 weeks effort).
- **License for Dev**: Developers need SQL Server tooling. Mitigation: Use SQL Server LocalDB (free, included with Visual Studio) or Docker containers.

## Consequences

### Positive

1. **Type-Safe Data Access**: EF Core's LINQ provider catches query errors at compile time, preventing runtime SQL exceptions.
2. **Automatic Migrations**: `dotnet ef migrations add` generates migrations from model changes; `dotnet ef database update` applies them.
3. **Built-in Audit Trails**: Temporal tables automatically track all changes to lead data for compliance (7-year retention requirement).
4. **Performance Monitoring**: Azure SQL Insights provides automatic query performance recommendations and index tuning.
5. **Disaster Recovery**: Automated geo-redundant backups with point-in-time restore (up to 35 days).
6. **Seamless Scaling**: Scale from Basic tier ($5/month) in dev to Standard/Premium tiers in production without code changes.
7. **Connection Resilience**: EF Core's built-in retry policies handle transient Azure SQL errors automatically.

### Negative

1. **Vendor Lock-in**: Azure SQL-specific features (temporal tables, JSON, full-text search) reduce portability to other databases.
2. **Cost at Scale**: If load exceeds projections (e.g., 10M+ leads/year), Azure SQL costs could become significant (~$500+/month for Premium tier).
3. **Cold Start Latency**: Serverless tier has ~1 second delay after auto-pause (not an issue for production with always-on Standard tier).
4. **Connection Pool Limits**: Basic tier limited to 30 concurrent connections (requires Standard tier for 100+ connections).

### Neutral

1. **Migration Complexity**: EF Core migrations require careful review of generated SQL scripts before production deployment (standard practice).
2. **Development Environment**: Developers can use SQL Server LocalDB (Windows), Docker (Mac/Linux), or Azure SQL free tier (cross-platform).
3. **Backup Strategy**: Automated backups enabled by default; no custom backup scripts needed.

## Implementation Notes

### Database Tier Selection

**Development Environment**:
- **Option A**: SQL Server LocalDB (Windows developers)
- **Option B**: SQL Server 2022 Docker container (Mac/Linux developers)
- **Option C**: Azure SQL Database Free Tier (cross-platform, requires Azure subscription)

**Staging/Production**:
- **Staging**: Azure SQL Database Serverless (2 vCores, 32GB max storage, auto-pause)
  - Cost: ~$30/month (auto-pauses when idle)
- **Production**: Azure SQL Database Standard S2 (50 DTU, 250GB storage)
  - Cost: ~$150/month
  - Performance: 90 DTU ~= 500 transactions/second (sufficient for 500 leads/minute)

**Scaling Path**:
- Start with Basic ($5/month) during development
- Upgrade to Standard S0 ($15/month) for QA testing
- Upgrade to Standard S2 ($150/month) for production launch
- Monitor DTU usage; upgrade to S3/S4 if DTU % consistently > 80%

### Entity Framework Core Configuration

**Package References** (for .NET 8 project):
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
```

**DbContext Registration** (Program.cs):
```csharp
builder.Services.AddDbContext<MarketingAgentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60);
        }));
```

**Connection String Format** (appsettings.json):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:<server>.database.windows.net,1433;Database=marketing-agent-db;User ID=<admin>;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

### Migration Strategy

**Migration Workflow**:
1. Define/modify entity models in `Models/` folder
2. Run `dotnet ef migrations add <MigrationName>` to generate migration
3. Review generated migration in `Migrations/` folder
4. Test migration on local database: `dotnet ef database update`
5. Commit migration to source control
6. CI/CD pipeline applies migration to staging/production during deployment

**Migration Naming Convention**:
- Use descriptive names: `InitialCreate`, `AddLeadScoringTables`, `AddTemporalTableSupport`
- Prefix with timestamp for ordering: `20251212_InitialCreate`

**Rollback Strategy**:
- Generate down migrations: `dotnet ef migrations script --from <PreviousMigration> --to <CurrentMigration>`
- Test rollback on staging before production
- Keep previous migration scripts in `Migrations/Scripts/` for emergency rollback

### Temporal Table Implementation

**Enable Temporal Tables** (for audit trails):
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Lead>(entity =>
    {
        entity.ToTable("Leads", b => b.IsTemporal(temporal =>
        {
            temporal.HasPeriodStart("ValidFrom");
            temporal.HasPeriodEnd("ValidTo");
            temporal.UseHistoryTable("LeadsHistory");
        }));
    });
}
```

**Query Temporal Data**:
```csharp
// Get all historical versions of a lead
var history = context.Leads
    .TemporalAll()
    .Where(l => l.Id == leadId)
    .OrderBy(l => EF.Property<DateTime>(l, "ValidFrom"))
    .ToList();

// Get lead data as of a specific date
var leadSnapshot = context.Leads
    .TemporalAsOf(new DateTime(2025, 1, 1))
    .FirstOrDefault(l => l.Id == leadId);
```

### Repository Pattern Implementation

**Base Repository Interface**:
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

**Optimistic Concurrency**:
```csharp
public class Lead
{
    public Guid Id { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; } // Optimistic concurrency token
    
    // Other properties...
}
```

### Performance Optimization

**Indexes** (configured in OnModelCreating):
```csharp
modelBuilder.Entity<Lead>(entity =>
{
    entity.HasIndex(l => l.Email).HasDatabaseName("IX_Lead_Email");
    entity.HasIndex(l => l.ExternalLeadId).IsUnique().HasDatabaseName("IX_Lead_ExternalLeadId");
    entity.HasIndex(l => l.Postcode).HasDatabaseName("IX_Lead_Postcode");
    entity.HasIndex(l => new { l.CreatedAt, l.Channel }).HasDatabaseName("IX_Lead_CreatedAt_Channel");
});

modelBuilder.Entity<LeadSourceAttribution>(entity =>
{
    entity.HasIndex(a => a.Channel).HasDatabaseName("IX_LeadSourceAttribution_Channel");
    entity.HasIndex(a => new { a.Channel, a.CapturedAt }).HasDatabaseName("IX_LeadSourceAttribution_Channel_CapturedAt");
});
```

**Query Optimization**:
- Use `.AsNoTracking()` for read-only queries (improves performance by 30-50%)
- Use `.Include()` for eager loading related entities (avoids N+1 queries)
- Use `.Select()` to project only needed fields (reduces memory usage)
- Use `.AsSplitQuery()` for complex includes (prevents cartesian explosion)

### Testing Strategy

**Unit Tests** (using in-memory provider):
```csharp
var options = new DbContextOptionsBuilder<MarketingAgentDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb")
    .Options;

using var context = new MarketingAgentDbContext(options);
var repository = new Repository<Lead>(context);
// Test repository methods...
```

**Integration Tests** (using Testcontainers):
```csharp
[Fact]
public async Task SaveLead_PersistsToDatabase()
{
    // Arrange
    await using var container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    
    await container.StartAsync();
    
    var connectionString = container.GetConnectionString();
    var options = new DbContextOptionsBuilder<MarketingAgentDbContext>()
        .UseSqlServer(connectionString)
        .Options;
    
    using var context = new MarketingAgentDbContext(options);
    await context.Database.MigrateAsync(); // Apply migrations
    
    var unitOfWork = new UnitOfWork(context);
    
    // Act
    var lead = new Lead { Email = "test@example.com" };
    await unitOfWork.Leads.AddAsync(lead);
    await unitOfWork.SaveChangesAsync();
    
    // Assert
    var savedLead = await unitOfWork.Leads.GetByIdAsync(lead.Id);
    Assert.NotNull(savedLead);
    Assert.Equal("test@example.com", savedLead.Email);
}
```

### Security Configuration

**Connection String Security**:
- Store connection strings in Azure Key Vault (production)
- Use Managed Identity for Azure SQL authentication (no passwords)
- Rotate credentials every 90 days (automated via Azure AD)

**Data Protection**:
- Transparent Data Encryption (TDE): Enabled by default on Azure SQL
- Encryption in Transit: Enforce TLS 1.2 (`Encrypt=True` in connection string)
- Column-Level Encryption: Use Always Encrypted for sensitive fields (future enhancement)

**Access Control**:
- Principle of least privilege: Application uses db_datareader, db_datawriter roles only
- No db_owner or sysadmin permissions for application service principal
- Separate read-only user for reporting/analytics dashboards

### Monitoring and Diagnostics

**Application Insights Integration**:
```csharp
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<MarketingAgentDbContext>(options =>
{
    options.UseSqlServer(connectionString)
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        .EnableDetailedErrors(builder.Environment.IsDevelopment());
});
```

**Query Performance Monitoring**:
- Enable Query Store on Azure SQL Database (automatic query performance insights)
- Monitor slow queries (> 1 second) in Application Insights
- Set up alerts for DTU usage > 80% (scale-up indicator)

**Health Checks**:
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MarketingAgentDbContext>("database");
```

## References

- [ADR-0001: Technology Stack Selection](./0001-technology-stack-selection.md) - .NET 8 with ASP.NET Core decision
- [Task 003: Data Persistence Layer](../tasks/003-task-data-persistence-layer.md) - Implementation requirements
- [FRD-001: Lead Capture Integration](../features/lead-capture-integration.md) - Lead data schema
- [FRD-002: Lead Qualification Routing](../features/lead-qualification-routing.md) - Lead scoring data requirements
- [FRD-003: Analytics Reporting](../features/analytics-reporting.md) - Analytics query requirements
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Azure SQL Database Documentation](https://learn.microsoft.com/en-us/azure/azure-sql/database/)
- [Temporal Tables in SQL Server](https://learn.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables)
- [EF Core Performance Best Practices](https://learn.microsoft.com/en-us/ef/core/performance/)
- [Azure SQL Database Pricing](https://azure.microsoft.com/en-us/pricing/details/azure-sql-database/)
