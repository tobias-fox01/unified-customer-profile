namespace unified_customer_profile.Test.Unit.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using unified_customer_profile.Api.Controllers;
using unified_customer_profile.Api.Dtos;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Test.Builders;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _customerService;
    private readonly Mock<ILogger<CustomerController>> _logger;

    private readonly CustomerController _customerController;
    private readonly IMapper _mapper;

    public CustomerControllerTests()
    {
        _customerService = new Mock<ICustomerService>();
        _logger = new Mock<ILogger<CustomerController>>();

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Address, AddressDto>();
            cfg.CreateMap<ExternalIds, ExternalIdsDto>();
            cfg.CreateMap<Customer, CustomerDto>();
        }, NullLoggerFactory.Instance));
        _customerController = new CustomerController(_customerService.Object, _logger.Object, _mapper);
    }

    [Fact]
    public async Task GetCustomer_WithCustomerId_ReturnsCustomerDto()
    {
        // Arrange
        var id = "cust-001";
        var customer = CustomerBuilder.Build();
        _customerService.Setup(x => x.GetCustomer(id)).ReturnsAsync(customer);

        // Act
        var result = await _customerController.Get(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CustomerDto>(okResult.Value) as CustomerDto;
        Assert.Equal(id, response.Id);
        Assert.Equal(customer.FirstName, response.FirstName);
        _customerService.Verify(s => s.GetCustomer(id), Times.Once);
    }

    [Fact]
    public async Task GetCustomer_WithWrongId_ReturnsNotFound()
    {
        // Arrange
        var id = "cust-a";
        _customerService.Setup(x => x.GetCustomer(id)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _customerController.Get(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
    }
}