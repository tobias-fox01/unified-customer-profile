namespace unified_customer_profile.Repository.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using unified_customer_profile.Shared.Config;

public class CosmosRepository<T> : ICosmosRepository<T>
{
    private readonly Container _container;
    private readonly string _containerId;
    private readonly Database _database;
    private readonly string _databaseId;
    private readonly ILogger<CosmosRepository<T>> _logger;

    public CosmosRepository(ILogger<CosmosRepository<T>> logger, IOptions<CosmosDBSettings> optionsCosmosDB, IOptionsMonitor<ContainerSettings> optionsContainer)
    {
        _logger = logger;
        
        // Initalise Cosmos Client
        CosmosClientOptions options = new()
        {
            ConnectionMode = ConnectionMode.Gateway,
            LimitToEndpoint = true,
            HttpClientFactory = () => new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            })
        };

        if (optionsCosmosDB.Value.Host is null)
        {
            throw new Exception("CosmosDB Host is not set.");
        }
        if (optionsCosmosDB.Value.AccountKey is null)
        {
            throw new Exception("CosmosDB Account Key is not set.");
        }
        CosmosClient client = new(optionsCosmosDB.Value.Host, optionsCosmosDB.Value.AccountKey, options);

        // Gets container from database using the ids
        ContainerSettings containerSettings = optionsContainer.Get(typeof(T).Name);
        if (containerSettings.DatabaseId is null)
        {
            throw new Exception("Database Id is not set.");
        }
        if (containerSettings.ContainerId is null)
        {
            throw new Exception("Container Id is not set.");
        }
        _databaseId = containerSettings.DatabaseId;
        _database = client.GetDatabase(_databaseId);
        _containerId = containerSettings.ContainerId;
        _container = _database.GetContainer(_containerId);
    }

    public async Task<T> GetItemFromContainer(string id)
    {
        _logger.LogDebug("Getting item with id: {id} from {container} container in {database}", id, _containerId, _databaseId);
        T item = await _container.ReadItemAsync<T>(id, new PartitionKey(id));

        _logger.LogDebug("Successful got item with id: {id} from {container} container in {database}", id, _containerId, _databaseId);
        return item;
    }
}