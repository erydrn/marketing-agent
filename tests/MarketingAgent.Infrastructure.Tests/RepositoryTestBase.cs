using Microsoft.EntityFrameworkCore;
using MarketingAgent.Infrastructure.Data;

namespace MarketingAgent.Infrastructure.Tests;

/// <summary>
/// Base class for repository tests providing in-memory database setup
/// </summary>
public abstract class RepositoryTestBase : IDisposable
{
    protected MarketingAgentDbContext Context { get; }
    
    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<MarketingAgentDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        Context = new MarketingAgentDbContext(options);
        Context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
