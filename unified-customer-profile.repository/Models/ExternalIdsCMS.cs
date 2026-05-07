namespace unified_customer_profile.repository.Models;

using Newtonsoft.Json;

public class ExternalIdsCMS
{
    [JsonProperty("marketing")]
    public string Marketing { get; set; }

    [JsonProperty("auth")]
    public string Auth { get; set; }

    [JsonProperty("loyalty")]
    public string Loyalty { get; set; }
}