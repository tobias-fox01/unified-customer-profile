namespace unified_customer_profile.Repository.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net;
using unified_customer_profile.Shared.Config;

public class CosmosRepository<T> : ICosmosRepository<T> where T : class
{
    private readonly Container _container;
    private readonly string _containerId;
    private readonly Database _database;
    private readonly string _databaseId;
    private readonly ILogger<CosmosRepository<T>> _logger;

    public CosmosRepository(CosmosClient client, ILogger<CosmosRepository<T>> logger, IOptionsMonitor<ContainerSettings> optionsContainer)
    {
        _logger = logger;

        ContainerSettings containerSettings = optionsContainer.Get(typeof(T).Name);
        // Gets container from database using the ids
        if (String.IsNullOrWhiteSpace(containerSettings.DatabaseId))
        {
            throw new ArgumentNullException(nameof(containerSettings.DatabaseId), "Database Id is not set.");
        }
        _databaseId = containerSettings.DatabaseId;
        _database = client.GetDatabase(_databaseId);
        if (String.IsNullOrWhiteSpace(containerSettings.ContainerId))
        {
            throw new ArgumentNullException(nameof(containerSettings.ContainerId), "Container Id is not set.");
        }
        _containerId = containerSettings.ContainerId;
        _container = _database.GetContainer(_containerId);
    }

    public async Task<T?> GetItemFromContainer(string id)
    {
        _logger.LogTrace("Getting item with id: {id} from {container} container in {database}", id, _containerId, _databaseId);

        try
        {
            T item = await _container.ReadItemAsync<T>(id, new PartitionKey(id));

            _logger.LogTrace("Successful got item with id: {id} from {container} container in {database}", id, _containerId, _databaseId);
            return item;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning("Item with id: {id} not found in {container} container in {database}", id, _containerId, _databaseId);
            return null;
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the item with id: {id}", id);
            throw new Exception("An error occurred while fetching the item.", ex);
        }
    }
}