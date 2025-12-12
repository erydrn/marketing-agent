using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MarketingAgent.Infrastructure.Data;

/// <summary>
/// Design-time factory for creating DbContext instances during migrations
/// </summary>
public class MarketingAgentDbContextFactory : IDesignTimeDbContextFactory<MarketingAgentDbContext>
{
    public MarketingAgentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MarketingAgentDbContext>();
        
        // Use SQL Server for design-time migrations
        // This connection string is only used for generating migrations
        // Runtime connection string comes from appsettings.json
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MarketingAgentDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        
        return new MarketingAgentDbContext(optionsBuilder.Options);
    }
}
