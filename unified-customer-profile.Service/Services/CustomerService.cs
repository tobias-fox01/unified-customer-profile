namespace unified_customer_profile.Service.Services;

using AutoMapper;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;

public class CustomerService(ICosmosRepository<CustomerCMS> customerCMSRepository, IMapper mapper) : ICustomerService
{
    public async Task<Customer> GetCustomer(string id)
    {
        CustomerCMS customerCMS = await customerCMSRepository.GetItemFromContainer(id);

        Customer customer = mapper.Map<Customer>(customerCMS);

        return customer;
    }
}