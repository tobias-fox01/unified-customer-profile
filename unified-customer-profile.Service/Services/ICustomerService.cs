namespace unified_customer_profile.Service.Services;

using unified_customer_profile.Repository.Models;
using unified_customer_profile.Shared.Models;

public interface ICustomerService
{
    Task<Customer?> GetCustomer(string id);
}