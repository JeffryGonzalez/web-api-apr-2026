namespace Software.Api.Vendors.Models;

public record VendorSummaryItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}