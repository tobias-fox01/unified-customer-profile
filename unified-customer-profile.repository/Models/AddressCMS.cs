namespace unified_customer_profile.repository.Models;

using Newtonsoft.Json;

public class AddressCMS
{
    [JsonProperty("line1")]
    public string Line1 { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("postcode")]
    public string Postcode { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }
}