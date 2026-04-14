using Software.Api.Vendors.Models;

namespace Software.Api.Vendors.Data;

public class VendorEntity
{
    public required Guid Id { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required string CreatedBy { get; init; }
    
    public required string Name { get; init; }
    
    public required string Site { get; init; }
    public required VendorPointOfContact PointOfContact { get; init; }


}
