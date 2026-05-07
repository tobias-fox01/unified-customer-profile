using Microsoft.Extensions.DependencyInjection;
using unified_customer_profile.services.Service;

namespace unified_customer_profile.services;

public static class ServiceExtensions
{
    public static IServiceCollection AddMiddlewareServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
};