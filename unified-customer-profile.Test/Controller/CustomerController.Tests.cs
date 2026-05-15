namespace unified_customer_profile.Test.Controllers;

using AutoMapper;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using unified_customer_profile.Api.Controllers;
using unified_customer_profile.Api.Dtos;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;
using Microsoft.Extensions.Logging.Abstractions;

public class CustomerControllerTests
{
    private readonly CustomerController _customerController;
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;
    private readonly IMapper _mapper;

    public CustomerControllerTests()
    {
        _customerService = Mock.Of<ICustomerService>();
        _logger = Mock.Of<ILogger<CustomerController>>();
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Address, AddressDto>();
            cfg.CreateMap<ExternalIds, ExternalIdsDto>();
            cfg.CreateMap<Customer, CustomerDto>();
        }, NullLoggerFactory.Instance));
        _customerController = new CustomerController(_customerService, _logger, _mapper);
    }

    [Fact]
    public void GetCustomer_WithCustomerId_ReturnsCustomer()
    {
        // Act
        var id = "cust-001";

        // Arrange
        var response = _customerController.Get(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        var model = Assert.IsType<IEnumerable<CustomerDto>>(okResult.Value, exactMatch: false);
        Assert.NotNull(model);
    }
}