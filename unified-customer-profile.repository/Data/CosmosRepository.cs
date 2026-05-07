namespace unified_customer_profile.repository.Data;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Newtonsoft.Json;
using unified_customer_profile.repository.Models;

public class CosmosRepository : ICosmosRepository
{
    private Container? _container;
    private CosmosClient? _client;
    private Database? _database;

    private readonly CosmosClientOptions _options;
    private readonly ILogger<CosmosRepository> _logger;
    private readonly CancellationTokenSource _source;

    public CosmosRepository(ILogger<CosmosRepository> logger)
    {
        _logger = logger;
        _source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        _options = this.cosmosClientOptions();

        _logger.LogInformation("Initialising CosmosRepository");
        _client = new(
            "https://localhost:8081",
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            _options
        );
        _logger.LogInformation("CosmosClient created successfully");
    }

    private async Task initaliseCosmosInstance()
    {
        _logger.LogInformation("Creating database");
        _database = await _client.CreateDatabaseIfNotExistsAsync(
            id: "CMS",
            throughput: 400
        );
        _logger.LogInformation("Database created");

        _logger.LogInformation("Creating container");
        _container = await _database.CreateContainerIfNotExistsAsync(
            id: "customers",
            partitionKeyPath: "/id"
        );
        _logger.LogInformation("Container created");
    }

    private CosmosClientOptions cosmosClientOptions()
    {
        CosmosClientOptions options = new()
        {
            ConnectionMode = ConnectionMode.Gateway,
            LimitToEndpoint = true,
            HttpClientFactory = () => new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            })
        };

        return options;
    }

    public async Task CreateDatabase()
    {
        _logger.LogInformation("Starting Request");

        if (_container == null || _database == null)
        {
            await this.initaliseCosmosInstance();
        }

        string jsonString = await File.ReadAllTextAsync("../unified-customer-profile.repository/Data/cms.json");
        CMS cms = JsonConvert.DeserializeObject<CMS>(jsonString);

        var tasks = cms.Customers.Select(customer =>
            _container.UpsertItemAsync(customer, new PartitionKey(customer.Id))
        );

        await Task.WhenAll(tasks);
        _logger.LogInformation("Finished Request");
    }
};