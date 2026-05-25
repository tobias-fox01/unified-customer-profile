using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using unified_customer_profile.Setup;
using unified_customer_profile.Setup.Models;

IConfigurationRoot config = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "./unified-customer-profile.Api"))
    .AddJsonFile($"appsettings.json", optional: true)
    .AddJsonFile($"appsettings.Development.json", optional: true)
    .Build();

try
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

    string? cosmosHost = config["CosmosDB:Host"];
    string? cosmosAccountKey = config["CosmosDB:Host"];

    if (String.IsNullOrWhiteSpace(cosmosHost))
    {
        throw new Exception("CosmosDB Host is not set.");
    }
    if (String.IsNullOrWhiteSpace(cosmosAccountKey))
    {
        throw new Exception("CosmosDB Account Key is not set.");
    }
    CosmosClient client = new(config["CosmosDB:Host"], config["CosmosDB:AccountKey"], options);

    // Initalise Container for CMS Customers
    var customerCMSConfig = new ContainerConfig
    {
        DataFile = config["CMS:DataDir"],
        DatabaseId = config["CMS:Customers:DatabaseId"],
        ContainerId = config["CMS:Customers:ContainerId"],
        PartitionKeyPath = config["CMS:Customers:PartitionKeyPath"],
    };
    Console.WriteLine("Creating CMS Customers Container");
    await SetupCosmos.InitaliseContainer(client, customerCMSConfig);
    Console.WriteLine("Completed CMS Customers Container");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}