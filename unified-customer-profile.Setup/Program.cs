using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using unified_customer_profile.Setup;
using unified_customer_profile.Setup.Models;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.LocalHost.json")
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

    if (cosmosHost is null)
    {
        throw new Exception("CosmosDB Host is not set.");
    }
    if (cosmosAccountKey is null)
    {
        throw new Exception("CosmosDB Account Key is not set.");
    }
    CosmosClient client = new(config["CosmosDB:Host"], config["CosmosDB:AccountKey"], options);

    // Initalise Container for CMS Customers
    var customerCMSConfig = new ContainerConfig
    {
        DataFile = config["CMS:DataDir"],
        DatabaseId = config["CMS:DatabaseName"],
        ContainerId = config["CMS:Customers:ContainerName"],
        PartitionKey = config["CMS:Customers:PartitionKey"],
    };
    Console.WriteLine("Creating CMS Customers Container");
    await SetupCosmos.InitaliseContainer(client, customerCMSConfig);
    Console.WriteLine("Completed CMS Customers Container");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}