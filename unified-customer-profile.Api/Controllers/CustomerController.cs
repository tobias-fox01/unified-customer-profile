namespace unified_customer_profile.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;
using unified_customer_profile.Api.Dtos;
using AutoMapper;

[ApiController]
[Route("[controller]")]
public class CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, IMapper mapper) : ControllerBase
{
	[HttpGet("/customer/{id}", Name = "GetCustomer")]
	public async Task<ActionResult<CustomerDto>> Get([FromRoute] string id)
	{
        logger.LogDebug("Starting get customer request");
		Customer customer = await customerService.GetCustomer(id);

        CustomerDto response = mapper.Map<CustomerDto>(customer);
        logger.LogDebug("Successfully got customer {customer}.", customer);
        return response;
	}
}