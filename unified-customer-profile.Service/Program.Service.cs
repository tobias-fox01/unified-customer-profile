namespace unified_customer_profile.Services;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddMiddlewareServices(this IServiceCollection services)
    {
        // services.AddScoped<, >();

        return services;
    }
};