namespace unified_customer_profile.Repository.Repositories;

using unified_customer_profile.Repository.Models;

public interface ICustomerCmsRepository
{
    public Task<CustomerCms?> GetCustomerRecord(string customerId);
}