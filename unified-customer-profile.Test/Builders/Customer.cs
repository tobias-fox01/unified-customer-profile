namespace unified_customer_profile.Test.Builders;

using unified_customer_profile.Shared.Models;

public static class CustomerBuilder
{
    public static Customer Build()
    {
        return new Customer
        {
            Id = "cust-001",
            FirstName = "James",
            LastName = "Smith",
            Email = "james.smith@example.com",
            Phone = "+447700900001",
            DateOfBirth = new DateOnly(1985, 4, 12),
            Address =
            {
                Line1 = "12 King Street",
                City = "Manchester",
                Postcode = "M1 2AB",
                Country = "UK"
            },
            ExternalIds =
            {
                Marketing = "mkt-1001",
                Auth = "auth-abc-001",
                Loyalty = "loy-5001"
            },
            CreatedAt = new DateTime(2024, 1, 10, 10, 15, 30),
        };
    }
};