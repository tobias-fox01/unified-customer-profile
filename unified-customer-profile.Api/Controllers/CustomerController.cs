namespace unified_customer_profile.Api.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using unified_customer_profile.Api.Dtos;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;

[ApiController]
[Route("[controller]")]
public class CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, IMapper mapper) : ControllerBase
{
    [HttpGet("/customer/{id}", Name = "GetCustomer")]
    public async Task<ActionResult<CustomerDto>> Get([FromRoute] string id)
    {
        logger.LogDebug("Starting get customer request");
        Customer customer = await customerService.GetCustomer(id);

        if (customer is null)
        {
            logger.LogWarning("Customer with id {id} not found.", id);
            return NotFound();
        };

        CustomerDto response = mapper.Map<CustomerDto>(customer);
        logger.LogDebug("Successfully got customer {customer}.", customer);
        return response;
    }
}