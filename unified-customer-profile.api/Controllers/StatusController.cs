using Microsoft.AspNetCore.Mvc;

namespace unified_customer_profile.api.Controllers
{
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
}
