namespace unified_customer_profile.Repository.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Shared.Config;

public class CustomerCmsRepository : ICustomerCmsRepository
{
    private readonly ICosmosRepository<CustomerCms> _cosmosRepository;
    private readonly ILogger<CustomerCmsRepository> _logger;

    public CustomerCmsRepository(ICosmosRepository<CustomerCms> cosmosRepository, ILogger<CustomerCmsRepository> logger)
    {
        _logger = logger;
        _cosmosRepository = cosmosRepository;
    }

    public async Task<CustomerCms?> GetCustomerRecord(string customerId)
    {
        _logger.LogTrace("Getting customer with id: {customerId} from CMS", customerId);
        CustomerCms? customerRecord = await _cosmosRepository.GetItemFromContainer(customerId);

        if (customerRecord is null)
        {
            _logger.LogWarning("Customer with id: {customerId} not found in CMS", customerId);
            return null;
        }

        _logger.LogTrace("Successful got customer with id: {customerId} from CMS", customerId);
        return customerRecord;
    }
}