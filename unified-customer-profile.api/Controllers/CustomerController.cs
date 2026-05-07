namespace unified_customer_profile.api.Controllers.Customer;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using unified_customer_profile.services.Service;

[ApiController]
[Route("[controller]")]
public class CustomerController(ICustomerService customerService, ILogger<CustomerController> logger) : ControllerBase
{
    [HttpGet(Name = "GetCustomer")]
    public async Task<IActionResult> Get()
    {
        logger.LogInformation("Started Controller");
        await customerService.CreateDatabase();
        logger.LogInformation("Finished Controller");

        return Ok();
    }
}