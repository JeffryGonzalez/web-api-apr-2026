namespace Software.Api.Vendors.Models;

public record VendorPointOfContact
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; set; }
}
