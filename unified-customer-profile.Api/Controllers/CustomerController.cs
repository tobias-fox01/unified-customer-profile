namespace unified_customer_profile.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using unified_customer_profile.Service.Services;
using unified_customer_profile.Shared.Models;

[ApiController]
[Route("[controller]")]
public class CustomerController(ILogger<CustomerController> logger, ICustomerService customerService) : ControllerBase
{
	[HttpGet(Name = "GetCustomer")]
	public async Task<ActionResult<Customer>> Get([FromRoute] string id)
	{
		Customer customer = await customerService.GetCustomer(id);

		return customer;
	}
}