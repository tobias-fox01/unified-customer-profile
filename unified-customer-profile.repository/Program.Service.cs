using Microsoft.Extensions.DependencyInjection;
using unified_customer_profile.repository.Data;

namespace unified_customer_profile.repository;

public static class ServiceExtensions
{
    public static IServiceCollection AddMiddlewareRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICosmosRepository, CosmosRepository>();

        return services;
    }
};