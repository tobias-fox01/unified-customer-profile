namespace unified_customer_profile.Test.Controllers;

using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using unified_customer_profile.Api.Controllers;

public class StatusControllerTest
{
	private ILogger<StatusController> _logger;
	private StatusController _statusController;

	public StatusControllerTest()
	{
		_logger = new NullLogger<StatusController>();
		_statusController = new StatusController(_logger);
	}

	[Fact]
	public void GetStatus_ReturnsOk()
	{
		// Act
		var response = _statusController.Get();

		// Assert
		var okResult = Assert.IsType<OkResult>(response);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
	}
}