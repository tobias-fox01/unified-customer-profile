namespace unified_customer_profile.repository.Models;

using Newtonsoft.Json;

public class CustomerCMS
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("firstName")]
	public string FirstName { get; set; }

	[JsonProperty("lastName")]
	public string LastName { get; set; }

	[JsonProperty("email")]
	public string Email { get; set; }

	[JsonProperty("phone")]
	public string Phone { get; set; }

	[JsonProperty("dateOfBirth")]
	public DateOnly DateOfBirth { get; set; }

	[JsonProperty("address")]
	public AddressCMS Address { get; set; }

	[JsonProperty("externalIds")]
	public ExternalIdsCMS ExternalIds { get; set; }

	[JsonProperty("createdAt")]
	public DateTime CreatedAt { get; set; }
}