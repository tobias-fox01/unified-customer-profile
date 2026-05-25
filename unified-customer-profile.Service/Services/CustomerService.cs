namespace unified_customer_profile.Service.Services;

using AutoMapper;
using Microsoft.Extensions.Logging;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Shared.Models;

public class CustomerService(ICustomerCmsRepository customerCmsRepository, ILogger<CustomerService> logger, IMapper mapper) : ICustomerService
{
    public async Task<Customer?> GetCustomer(string id)
    {
        logger.LogDebug("Getting customer from CMS Customers container.");
        CustomerCms? customerCms = await customerCmsRepository.GetCustomerRecord(id);

        if (customerCms == null)
        {
            logger.LogWarning("Customer with id {id} not found.", id);
            return null;
        }

        Customer customer = mapper.Map<Customer>(customerCms);
        logger.LogDebug("Successfully got customer {@customer} from CMS Customers container.", customer);

        return customer;
    }
}