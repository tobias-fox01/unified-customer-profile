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

    public CosmosRepository(ILogger<CosmosRepository<T>> logger, IOptions<CosmosDBSettings> optionsCosmosDB, string containerId, string databaseId)
    {
        _logger = logger;
        _containerId = containerId;
        _databaseId = databaseId;

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
        _databaseId = databaseId;
        _database = client.GetDatabase(_databaseId);
        _containerId = containerId;
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
            resultStatus = Exception;
            throw new Exception("The server encountered an internal error.");
        }
    }
}