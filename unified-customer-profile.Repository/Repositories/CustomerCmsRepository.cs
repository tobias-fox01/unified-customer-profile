namespace unified_customer_profile.Repository.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Shared.Config;

public class CustomerCmsRepository : ICustomerCmsRepository
{
    private readonly ICosmosRepository<CustomerCMS> _cosmosRepository;
    private readonly ILogger<CustomerCmsRepository> _logger;

    public CustomerCmsRepository(ILogger<CustomerCmsRepository> logger, ILogger<CosmosRepository<CustomerCMS>> superLogger, IOptions<CosmosDBSettings> optionsCosmosDB, IOptionsMonitor<ContainerSettings> optionsContainer)
    {
        _logger = logger;

        ContainerSettings customerContainerSettings = optionsContainer.Get("CustomerCMS");
        if (customerContainerSettings.DatabaseId is null)
        {
            throw new Exception("Database Id is not set.");
        }
        if (customerContainerSettings.ContainerId is null)
        {
            throw new Exception("Container Id is not set.");
        }
        _cosmosRepository = new CosmosRepository<CustomerCMS>(superLogger, optionsCosmosDB, customerContainerSettings.ContainerId, customerContainerSettings.DatabaseId);
    }

    public async Task<CustomerCMS> GetCustomer(string customerId)
    {
        _logger.LogTrace("Getting customer with id: {customerId} from CMS", customerId);
        CustomerCMS customer = await _cosmosRepository.GetItemFromContainer(customerId);

        _logger.LogTrace("Successful got customer with id: {customerId} from CMS", customerId);
        return customer;
    }
}