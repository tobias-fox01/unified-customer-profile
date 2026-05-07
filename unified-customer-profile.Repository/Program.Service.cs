namespace unified_customer_profile.Repository;

using Microsoft.Extensions.DependencyInjection;


public static class ServiceExtensions
{
    public static IServiceCollection AddMiddlewareRepositories(this IServiceCollection services)
    {
        // services.AddScoped<, >();

        return services;
    }
};