namespace unified_customer_profile.Test.Unit.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Shared.Config;
using unified_customer_profile.Test.Builders;

public class CosmosRepositoryTests
{
    private readonly Mock<ILogger<CosmosRepository<CustomerCms>>> _logger;
    private readonly Mock<IOptionsMonitor<ContainerSettings>> _optionsContainer;
    private readonly Mock<CosmosClient> _client;
    private readonly Mock<Database> _database;
    private readonly Mock<Container> _container;

    private readonly CosmosRepository<CustomerCms> _cosmosRepository;

    public CosmosRepositoryTests()
    {
        _logger = new Mock<ILogger<CosmosRepository<CustomerCms>>>();

        // Mocking CosmosDB Client, Database, and Container
        _client = new Mock<CosmosClient>();
        _database = new Mock<Database>();
        _container = new Mock<Container>();
        _client.Setup(x => x.GetDatabase(It.IsAny<string>()))
              .Returns(_database.Object);
        _database.Setup(x => x.GetContainer(It.IsAny<string>()))
            .Returns(_container.Object);

        // Setup mock options for CosmosDB and ContainerSettings
        _optionsContainer = new Mock<IOptionsMonitor<ContainerSettings>>();
        _optionsContainer
            .Setup(x => x.Get("CustomerCms"))
            .Returns(new ContainerSettings
            {
                DatabaseId = "CMS",
                ContainerId = "customers"
            });

        _cosmosRepository = new CosmosRepository<CustomerCms>(_client.Object, _logger.Object, _optionsContainer.Object);
    }

    [Fact]
    public async Task GetItemFromContainer_WithCorrectId_ReturnsItem()
    {
        // Arrange
        string id = "cust-001";
        CustomerCms customer = CustomerCmsBuilder.Build();

        var mockResponse = new Mock<ItemResponse<CustomerCms>>();
        mockResponse.Setup(r => r.Resource).Returns(customer);

        _container
            .Setup(r => r.ReadItemAsync<CustomerCms>(id, new PartitionKey(id)))
            .ReturnsAsync(mockResponse.Object);

        // Act
        var result = await _cosmosRepository.GetItemFromContainer(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(customer.FirstName, result.FirstName);
        _container.Verify(r => r.ReadItemAsync<CustomerCms>(id, new PartitionKey(id)), Times.Once);
    }

    [Fact]
    public async Task GetItemFromContainer_WithInvalidId_ReturnsNull()
    {
        // Arrange
        string id = "cust-a";
        _container
            .Setup(r => r.ReadItemAsync<CustomerCms>(id, new PartitionKey(id)))
            .ThrowsAsync(new CosmosException("Not Found", HttpStatusCode.NotFound, 404, "test-not-found", 0));

        // Act
        var result = await _cosmosRepository.GetItemFromContainer(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetItemFromContainer_WhenCosmosExceptionThrown_ReturnsException()
    {
        // Arrange
        string id = "cust-a";
        _container
            .Setup(r => r.ReadItemAsync<CustomerCms>(id, new PartitionKey(id)))
            .ThrowsAsync(new CosmosException("Unexpected error", HttpStatusCode.InternalServerError, 500, "test-not-found", 0));

        // Act
        var result = _cosmosRepository.GetItemFromContainer(id);

        // Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => result);
        Assert.Equal("An error occurred while fetching the item.", exception.Message);
    }
}