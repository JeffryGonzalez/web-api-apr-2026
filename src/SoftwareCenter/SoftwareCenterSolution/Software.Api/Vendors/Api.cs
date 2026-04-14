using Microsoft.AspNetCore.Mvc;

namespace Software.Api.Vendors;

public class Api : ControllerBase
{
    [HttpPost("/vendors")]
    public async Task<ActionResult> AddVendorAsync([FromBody] Vendor request)
    {

        var newResponse = request with { Name = request.Name.ToUpper() };
      
        return Ok(newResponse); 
    }
}

/*
 * {
    "name": "Microsoft",
    "site": "https://www.microsoft.com",
    "pointOfContact": {
        "name": "Satya Nadella",
        "email": "satya@microsoft.com",
        "phone": "888-5555"
    }
   
}
*/

public record Vendor
{
    public required string Name { get; init; }
    public required string site { get; init; }
    public required VendorPointOfContact PointOfContact { get; init; }
}

public record VendorPointOfContact
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; set; }
}