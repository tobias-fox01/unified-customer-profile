namespace unified_customer_profile.Repository.Repositories;

public interface ICosmosRepository<T>
{
    public Task<T?> GetItemFromContainer(string id);
}