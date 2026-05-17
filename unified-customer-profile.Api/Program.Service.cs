namespace unified_customer_profile.Api;

using AutoMapper;
using unified_customer_profile.Api.Mapper;

public static class ServiceExtensions
{
    public static IServiceCollection InitalizeAutoMapper(this IServiceCollection services)
    {
        // Add mapper to service
        services.AddAutoMapper(MapperConfigurationExtensions.Configure);

        return services;
    }
};