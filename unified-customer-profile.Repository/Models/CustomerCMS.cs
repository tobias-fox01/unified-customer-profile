namespace unified_customer_profile.Repository.Models;

public class CustomerCMS
{
    public string? Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public AddressCMS? Address { get; set; }

    public ExternalIdsCMS? ExternalIds { get; set; }

    public DateTime CreatedAt { get; set; }
}