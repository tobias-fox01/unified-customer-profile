namespace unified_customer_profile.Repository;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Shared.Config;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationConfig(this IServiceCollection services, IConfiguration cfg)
    {
        // Add config into service
        services.Configure<CosmosDBSettings>(cfg.GetSection("CosmosDB"));
        services.Configure<ContainerSettings>("CustomerCms", cfg.GetSection("CMS:Customers"));

        return services;
    }

    public static IServiceCollection AddMiddlewareRepositories(this IServiceCollection services)
    {
        // Add scopes for repositories
        services.AddSingleton<CosmosClient>((s) => {
            // Initalise Cosmos Client
            CosmosDBSettings optionsCosmosDB = s.GetRequiredService<IOptions<CosmosDBSettings>>().Value;
            CosmosClientOptions options = new()
            {
                ConnectionMode = ConnectionMode.Gateway,
                LimitToEndpoint = true,
                HttpClientFactory = () => new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
            };

            if (String.IsNullOrWhiteSpace(optionsCosmosDB.Host))
            {
                throw new Exception("CosmosDB Host is not set.");
            }
            if (String.IsNullOrWhiteSpace(optionsCosmosDB.AccountKey))
            {
                throw new Exception("CosmosDB Account Key is not set.");
            }
            CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(optionsCosmosDB.Host, optionsCosmosDB.AccountKey);
            return configurationBuilder
                    .Build();
        });
        services.AddSingleton<ICosmosRepository<CustomerCms>, CosmosRepository<CustomerCms>>();
        services.AddSingleton<ICustomerCmsRepository, CustomerCmsRepository>();

        return services;
    }
};