namespace unified_customer_profile.Setup;

using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using unified_customer_profile.Setup.Models;

public static class SetupCosmos
{
    public static async Task InitaliseContainer(CosmosClient client, ContainerConfig config)
    {
        // Creates database if it doesnt already exist
        if (config.DatabaseId is null)
        {
            throw new Exception("CosmosDB Database Id is not set.");
        }
        Database database = await client.CreateDatabaseIfNotExistsAsync(
            id: config.DatabaseId,
            throughput: 400
        );

        // Creates container if it doesnt already exist]
        if (config.ContainerId is null)
        {
            throw new Exception("CosmosDB Container Id is not set.");
        }
        if (config.PartitionKey is null)
        {
            throw new Exception("Container's Partition Key is not set.");
        }
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: config.ContainerId,
            partitionKeyPath: config.PartitionKey
        );

        // Turns the json file into an array of items
        if (config.DataFile is null)
        {
            throw new Exception("Data file directory is not set.");
        }
        string jsonString = await File.ReadAllTextAsync(config.DataFile);
        JObject document = JObject.Parse(jsonString);
        JArray? items = document[config.ContainerId] as JArray;

        if (items is null)
        {
            throw new Exception($"Container data for {config.ContainerId} does not exist in file.");
        }

        // Upserts each item from the section of the file to CosmosDB
        foreach (JToken item in items)
        {
            string? id = item["id"]?.ToString();
            if (id is null)
            {
                throw new Exception("Item in data does not contain an id.");
            }
            await container.UpsertItemAsync(item, new PartitionKey(id));
        }
    }
}
