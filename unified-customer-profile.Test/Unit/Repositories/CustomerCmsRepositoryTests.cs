namespace unified_customer_profile.Test.Unit.Repositories;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Test.Builders;

public class CustomerCmsRepositoryTests
{
    private readonly Mock<ICosmosRepository<CustomerCms>> _cosmosRepository;
    private readonly Mock<ILogger<CustomerCmsRepository>> _logger;

    private readonly CustomerCmsRepository _customerCmsRepository;

    public CustomerCmsRepositoryTests()
    {
        _logger = new Mock<ILogger<CustomerCmsRepository>>();
        _cosmosRepository = new Mock<ICosmosRepository<CustomerCms>>();

        _customerCmsRepository = new CustomerCmsRepository(_cosmosRepository.Object, _logger.Object);
    }

    [Fact]
    public async Task GetCustomerRecord_WithCustomerId_ReturnsCustomerRecord()
    {
        // Arrange
        var id = "cust-001";
        var customer = CustomerCmsBuilder.Build();
        _cosmosRepository.Setup(x => x.GetItemFromContainer(id)).ReturnsAsync(customer);

        // Act
        var result = await _customerCmsRepository.GetCustomerRecord(id);

        // Assert
        var response = result.Should().BeOfType<CustomerCms>().Subject;
        response.Should().BeEquivalentTo(customer);
        _cosmosRepository.Verify(r => r.GetItemFromContainer("cust-001"), Times.Once);
    }

    [Fact]
    public async Task GetCustomerRecord_WithWrongId_ReturnsNull()
    {
        // Arrange
        var id = "cust-a";
        _cosmosRepository.Setup(x => x.GetItemFromContainer(id)).ReturnsAsync((CustomerCms?)null);

        // Act
        var result = await _customerCmsRepository.GetCustomerRecord(id);

        // Assert
        result.Should().BeNull();
    }
}