namespace unified_customer_profile.Test.Unit.Services;

using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using unified_customer_profile.Repository.Models;
using unified_customer_profile.Repository.Repositories;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Test.Builders;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerCmsRepository> _customerRepository;
    private readonly Mock<ILogger<CustomerService>> _logger;

    private readonly CustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerServiceTests()
    {
        _customerRepository = new Mock<ICustomerCmsRepository>();
        _logger = new Mock<ILogger<CustomerService>>();

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddressCms, Address>();
            cfg.CreateMap<ExternalIdsCms, ExternalIds>();
            cfg.CreateMap<CustomerCms, Customer>();
        }, NullLoggerFactory.Instance));
        _customerService = new CustomerService(_customerRepository.Object, _logger.Object, _mapper);
    }

    [Fact]
    public async Task GetCustomer_WithCustomerId_ReturnsCustomer()
    {
        // Arrange
        var id = "cust-001";
        var customer = CustomerCmsBuilder.Build();
        _customerRepository.Setup(x => x.GetCustomerRecord(id)).ReturnsAsync(customer);

        // Act
        var result = await _customerService.GetCustomer(id);

        // Assert
        var response = Assert.IsType<Customer>(result) as Customer;
        Assert.Equal(id, response.Id);
        Assert.Equal(customer.FirstName, response.FirstName);
        _customerRepository.Verify(r => r.GetCustomerRecord(id), Times.Once);
    }

    [Fact]
    public async Task GetCustomer_WithWrongId_ReturnsNull()
    {
        // Arrange
        var id = "cust-a";
        _customerRepository.Setup(x => x.GetCustomerRecord(id)).ReturnsAsync((CustomerCms?)null);

        // Act
        var result = await _customerService.GetCustomer(id);

        // Assert
        Assert.Null(result);
    }
}