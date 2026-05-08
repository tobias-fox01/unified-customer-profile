namespace unified_customer_profile.Repository;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using unified_customer_profile.Shared.Config;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Repository.Models;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Add config into service
        services.Configure<CosmosDBSettings>(configuration.GetSection("CosmosDB"));
        services.Configure<ContainerSettings>("CustomerCMS", configuration.GetSection("CMS:Customers"));

        return services;
    }

    public static IServiceCollection AddMiddlewareRepositories(this IServiceCollection services)
    {
        services.AddSingleton<CosmosRepository<CustomerCMS>>();

        return services;
    }
};