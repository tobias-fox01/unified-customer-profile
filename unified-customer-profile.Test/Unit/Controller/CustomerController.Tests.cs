namespace unified_customer_profile.Test.Unit.Controllers;

using AutoMapper;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using unified_customer_profile.Api.Controllers;
using unified_customer_profile.Api.Dtos;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Test.Models;
using Microsoft.Extensions.Logging.Abstractions;

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
    public void GetCustomer_WithCustomerId_ReturnsCustomer()
    {
        // Act
        var id = "cust-001";
        _customerService.Setup(x => x.GetCustomer(id)).ReturnsAsync(CustomerBuilder.Build());

        // Arrange
        var response = _customerController.Get(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        var model = Assert.IsType<IEnumerable<CustomerDto>>(okResult.Value, exactMatch: false);
        Assert.NotNull(model);
    }

    [Fact]
    public void GetCustomer_WithCustomerId_ReturnsCustomer()
    {
        // Act
        var id = "cust-001";
        _customerService.Setup(x => x.GetCustomer(id)).ReturnsAsync(CustomerBuilder.Build());

        // Arrange
        var response = _customerController.Get(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        var model = Assert.IsType<IEnumerable<CustomerDto>>(okResult.Value, exactMatch: false);
        Assert.NotNull(model);
    }

    [Fact]
    public void GetCustomer_WithWrongId_ReturnsNotFound()
    {
        // Act
        var id = "cust-001";
        _customerService.Setup(x => x.GetCustomer(id)).ReturnsAsync((Customer)null);

        // Arrange
        var response = _customerController.Get(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(response);
    }
}