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
        // Add scope to service
        services.AddScoped<ICustomerService, CustomerService>();

        // Add mapper to service
        services.AddAutoMapper(cfg => {
            cfg.CreateMap<CustomerCMS, Customer>();
        });

        return services;
    }
};