namespace unified_customer_profile.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase

{
    private readonly ILogger<StatusController> _logger;

    public StatusController(ILogger<StatusController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetStatus")]
    public IActionResult Get()
    {
        return Ok();
    }
}