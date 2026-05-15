namespace unified_customer_profile.Service.Services;

using AutoMapper;
using Microsoft.Extensions.Logging;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;

public class CustomerService(ICustomerCmsRepository customerCmsRepository, ILogger<CustomerService> logger, IMapper mapper) : ICustomerService
{
    public async Task<Customer> GetCustomer(string id)
    {
        logger.LogDebug("Getting customer from CMS Customers container.");
        CustomerCMS customerCMS = await customerCmsRepository.GetCustomer(id);

        Customer customer = mapper.Map<Customer>(customerCMS);
        logger.LogDebug("Successfully got customer from CMS Customers container.");

        return customer;
    }
}