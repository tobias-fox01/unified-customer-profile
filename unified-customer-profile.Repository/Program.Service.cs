namespace unified_customer_profile.Repository;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Shared.Config;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationConfig(this IServiceCollection services, IConfiguration cfg)
    {
        // Add config into service
        services.Configure<CosmosDBSettings>(cfg.GetSection("CosmosDB"));
        services.Configure<ContainerSettings>("CustomerCMS", cfg.GetSection("CMS:Customers"));

        return services;
    }

    public static IServiceCollection AddMiddlewareRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));

        return services;
    }
};