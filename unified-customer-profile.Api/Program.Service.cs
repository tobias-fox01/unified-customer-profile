namespace unified_customer_profile.Api;

using AutoMapper;
using unified_customer_profile.Api.Dtos;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Repository.Models;

public static class ServiceExtensions
{
    public static IServiceCollection InitalizeAutoMapper(this IServiceCollection services)
    {
        // Add mapper to service
        services.AddAutoMapper(cfg => {
            cfg.CreateMap<Address, AddressDto>();
            cfg.CreateMap<ExternalIds, ExternalIdsDto>();
            cfg.CreateMap<Customer, CustomerDto>();
            cfg.CreateMap<AddressCMS, Address>();
            cfg.CreateMap<ExternalIdsCMS, ExternalIds>();
            cfg.CreateMap<CustomerCMS, Customer>();
        });

        return services;
    }
};