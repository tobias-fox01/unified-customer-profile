namespace unified_customer_profile.Service;

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Shared.Models;

public static class ServiceExtensions
{
    public static IServiceCollection AddMiddlewareServices(this IServiceCollection services)
    {
        // Add scopes for services
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
};