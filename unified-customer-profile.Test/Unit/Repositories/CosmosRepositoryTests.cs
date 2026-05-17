namespace unified_customer_profile.Test.Unit.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Shared.Config;
using unified_customer_profile.Test.Builders;

public class CosmosRepositoryTests
{
    private readonly Mock<ILogger<CosmosRepository<CustomerCms>>> _logger;
    private readonly IOptions<CosmosDBSettings> _optionsCosmosDB;
    private readonly Mock<IOptionsMonitor<ContainerSettings>> _optionsContainer;

    private readonly CosmosRepository<CustomerCms> _cosmosRepository;

    public CosmosRepositoryTests()
    {
        _logger = new Mock<ILogger<CosmosRepository<CustomerCms>>>();

        // Setup mock for cosmos client
        var responseMock = new Mock<ItemResponse<ConnectionData>>();
        //data to be passed to the method under test
        ConnectionData data = new ConnectionData
        {
            ConnectionId = "some value here"
        };

        var containerMock = new Mock<Container>();

        // Setup mock options for CosmosDB and ContainerSettings
        _optionsCosmosDB = Options.Create(new CosmosDBSettings
        {
            Host = "mockHost",
            AccountKey = "mockAccountKey"
        });
        _optionsContainer = new Mock<IOptionsMonitor<ContainerSettings>>();
        _optionsContainer
            .Setup(x => x.Get("CustomerCms"))
            .Returns(new ContainerSettings
            {
                DatabaseId = "CMS",
                ContainerId = "customers"
            });

        _cosmosRepository = new CosmosRepository<CustomerCms>(_logger.Object, _optionsCosmosDB, _optionsContainer.Object);
    }

    [Fact]
    public async Task GetItemFromContainer_WithCorrectId_ReturnsItem()
    {
        // Arrange
        var id = "cust-001";
        var customer = CustomerCmsBuilder.Build();
        _cosmosRepository.Setup(x => x.GetItemFromContainer(id)).ReturnsAsync(customer);

        // Act
        var result = await _cosmosRepository.GetItemFromContainer(id);

        // Assert
        var response = Assert.IsType<CustomerCms>(result) as CustomerCms;
        Assert.Equal("cust-001", response.Id);
        Assert.Equal("James", response.FirstName);
        _cosmosRepository.Verify(r => r.GetItemFromContainer("cust-001"), Times.Once);
    }
}