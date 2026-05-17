namespace unified_customer_profile.Api.Mapper;

using AutoMapper;
using unified_customer_profile.Api.Dtos;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Shared.Models;

public static class MapperConfigurationExtensions
{
    public static void Configure(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Address, AddressDto>();
        cfg.CreateMap<ExternalIds, ExternalIdsDto>();
        cfg.CreateMap<Customer, CustomerDto>();
        cfg.CreateMap<AddressCms, Address>();
        cfg.CreateMap<ExternalIdsCms, ExternalIds>();
        cfg.CreateMap<CustomerCms, Customer>();
    }
}