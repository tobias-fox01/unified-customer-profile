namespace unified_customer_profile.repository.Models;

using Newtonsoft.Json;

public class CMS
{
    [JsonProperty("customers")]
    public List<CustomerCMS> Customers { get; set; }
}