using System.ComponentModel.DataAnnotations;

namespace Software.Api.Vendors.Models;

public record VendorCreateResponse
{
    public required Guid Id { get; init; }
    public required DateTimeOffset Created { get; init; }

    [MinLength(3), MaxLength(20)]
    public required string Name { get; init; }
    [Url]
    public required string Site { get; init; }
    public required VendorPointOfContact PointOfContact { get; init; }


}
