namespace unified_customer_profile.Repository.Repositories;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using unified_customer_profile.Shared.Config;

public class CosmosRepository<T> : ICosmosRepository<T>
{
    private readonly Container _container;
    private readonly Database _database;
    private readonly string _partitionKey;

    public CosmosRepository(IOptions<CosmosDBSettings> optionsCosmosDB, IOptionsMonitor<ContainerSettings> optionsContainer)
    {
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
        _database = client.GetDatabase(containerSettings.DatabaseId);
        _container = _database.GetContainer(containerSettings.ContainerId);

        // Sets the partition key for the repository
        if (containerSettings.PartitionKey is null)
        {
            throw new Exception("Partition key is not set.");
        }
        _partitionKey = containerSettings.PartitionKey;
    }

    public async Task<T> GetItemFromContainer(string id)
    {
        T item = await _container.ReadItemAsync<T>(id, new PartitionKey(_partitionKey));

        return item;
    }
}