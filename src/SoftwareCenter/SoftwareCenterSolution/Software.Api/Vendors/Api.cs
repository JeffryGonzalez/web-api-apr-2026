using Marten;
using Microsoft.AspNetCore.Mvc;
using Software.Api.Vendors.Data;
using Software.Api.Vendors.Models;
using Software.Api.Vendors.Services;

namespace Software.Api.Vendors;

[ApiController]
public class Api(IManageVendors vendorData) : ControllerBase
{

    [HttpGet("/vendors")]
    public async Task<ActionResult> GetVendors(CancellationToken token)
    {

        // DO NOT DO THIS. THIS IS CLASSROOM CODE. EVEN IF JEFF JOKES AND SAYS YOU SHOULD DO THIS, NO NOT.
        await Task.Delay(2000,token);
        IReadOnlyList<VendorSummaryItem> entities = await vendorData.GetVendorSummariesAsync(token);
        return Ok(entities);
    }

    [HttpGet("/vendors/{id:guid}")]
    public async Task<ActionResult> GetVendorById(Guid id, CancellationToken token)
    {
       VendorCreateResponse? entity = await vendorData.GetVendorByIdAsync(id, token);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost("/vendors")] // Is not safe, not idempotent, and not cacheable*
    public async Task<ActionResult> AddVendorAsync([FromBody] Vendor request)
    {
        // 1. Validation - field level - all the stuff there, in the right "range of acceptable values"
        // 2. More (deep) validation - TODO: We probably shouldn't allow multiple vendors with the same name.
        // 3. If 1 or 2 fails, return a 400, maybe with some error information.
        // 4. Must look good, do your "thang" 
        // 5. For us, save it into a database -- where the real monsters hide - why they pay you $$
        // 6. Consider using the idomatic approach for a post to a collection:

        // 7. Return a 201 Created Status Code
        // 8. Return a Location header with the absolute or relative URL of the new resource
        // 9. Return a copy of what they would get if they did a get on the Location header.


        var response = await vendorData.AddVendorAsync(request);
        

        return Created($"/vendors/{response.Id}", response);
    }
}
