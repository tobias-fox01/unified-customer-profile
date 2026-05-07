namespace unified_customer_profile.services.Service;

using Microsoft.Extensions.Logging;
using unified_customer_profile.repository.Data;

public class CustomerService(ICosmosRepository cosmosRepository, ILogger<CustomerService> logger) : ICustomerService
{
    public async Task CreateDatabase()
    {
        logger.LogInformation("Starting Service");

        await cosmosRepository.CreateDatabase();
    }
}