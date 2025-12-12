using FluentAssertions;
using MarketingAgent.Core.Entities;
using MarketingAgent.Core.Enums;
using MarketingAgent.Infrastructure.Repositories;

namespace MarketingAgent.Infrastructure.Tests.Repositories;

public class LeadRepositoryTests : RepositoryTestBase
{
    private readonly LeadRepository _repository;
    
    public LeadRepositoryTests()
    {
        _repository = new LeadRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddLead_AndReturnWithId()
    {
        // Arrange
        var lead = new Lead
        {
            ExternalLeadId = "EXT-001",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "+44 1234 567890",
            PropertyType = PropertyType.Residential,
            ServiceType = ServiceType.Conveyancing
        };
        
        // Act
        var result = await _repository.AddAsync(lead);
        await Context.SaveChangesAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.ExternalLeadId.Should().Be("EXT-001");
        result.FullName.Should().Be("John Doe");
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnLead_WhenExists()
    {
        // Arrange
        var lead = new Lead
        {
            ExternalLeadId = "EXT-002",
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com"
        };
        await _repository.AddAsync(lead);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetByIdAsync(lead.Id);
        
        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(lead.Id);
        result.Email.Should().Be("jane.smith@example.com");
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnLead_WhenEmailMatches()
    {
        // Arrange
        var lead = new Lead
        {
            ExternalLeadId = "EXT-003",
            FirstName = "Bob",
            LastName = "Johnson",
            Email = "bob@example.com"
        };
        await _repository.AddAsync(lead);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetByEmailAsync("bob@example.com");
        
        // Assert
        result.Should().NotBeNull();
        result!.ExternalLeadId.Should().Be("EXT-003");
    }
    
    [Fact]
    public async Task CountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
        {
            await _repository.AddAsync(new Lead
            {
                ExternalLeadId = $"EXT-{i}",
                FirstName = $"User{i}",
                LastName = "Test",
                Email = $"user{i}@example.com"
            });
        }
        await Context.SaveChangesAsync();
        
        // Act
        var count = await _repository.CountAsync();
        
        // Assert
        count.Should().Be(5);
    }
}
