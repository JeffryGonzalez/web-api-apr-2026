using Alba;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Software.Api.Vendors.Data;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;
using System.Security.Claims;
/*
 * This is an **integration test** that verifies the complete workflow of adding a vendor to the system.
 * Here's what it does step-by-step:

## Test Breakdown

### 1. **Setup - Create Test Data**
Creates a `Vendor` object with sample data (name, website, contact info).

### 2. **POST Request - Add the Vendor**
```csharp
api.Post.Json(vendorToAdd).ToUrl("/vendors");
api.StatusCodeShouldBe(201);
```
- Sends the vendor to the `/vendors` endpoint
- Expects a `201 Created` status code
- Captures the `Location` header (URL of the newly created resource)
- Reads the response as `VendorCreateResponse`

### 3. **GET Request - Retrieve the Vendor**
```csharp
api.Get.Url(locationHeader.ToString());
```
- Uses the `Location` header from step 2 to fetch the vendor
- Verifies the response is `200 OK`
- Compares the vendor from GET with the one from POST to ensure they match

### 4. **Database Verification** (The Cool Part!)
```csharp
using var sp = fixture.Host.Services.CreateScope();
using var db = sp.ServiceProvider.GetRequiredService<IDocumentSession>();
var savedVendor = await db.LoadAsync<VendorEntity>(...)
```
- Creates a new dependency injection scope (simulating a request)
- Gets a Marten `IDocumentSession` to query the database directly
- Loads the vendor entity from the database using its ID
- Verifies it exists and that `CreatedBy` is set to `"boba-fett"`

## Why This is Valuable

This test goes **beyond black-box API testing** by also verifying the database state directly - 
something you can't do with tools like Postman or Playwright.
It ensures the entire flow works: API → Business Logic → Database persistence.
*/
namespace Software.Tests.Vendors;

[Collection("VendorsSystemTests")]
[Trait("Category", "Vendors")]
[Trait("Category", "System")]

public class AddingAVendor(VendorsFixture fixture)
{

    [Fact]
    public async Task NonSoftwareCenterManagersCannotAddVendors()
    {
        var vendorToAdd = new Vendor
        {
            Name = "Test Vendor",
            Site = "https://testvendor.com",
            PointOfContact = new VendorPointOfContact
            {
                Email = "bob@testvendor.com",
                Name = "Bob Smith",
            }

        };
        // Add A Vendor
        var postResult = await fixture.Host.Scenario(api =>
        {
            api.WithClaim(new Claim("sub", "boba-fett")); // Simulate an authenticated user with a "sub" claim of "boba-fett"
           
            api.Post.Json(vendorToAdd).ToUrl("/vendors");
            api.StatusCodeShouldBe(403);
        });
    }

    [Fact]
    public async Task CanAddVendor()
    {
        var vendorToAdd = new Vendor
        {
            Name = "Test Vendor",
            Site = "https://testvendor.com",
            PointOfContact = new VendorPointOfContact
            {
                Email = "bob@testvendor.com",
                Name = "Bob Smith",
            }

        };
        // Add A Vendor
        var postResult = await fixture.Host.Scenario(api =>
        {
            api.WithClaim(new Claim("sub", "boba-fett")); // Simulate an authenticated user with a "sub" claim of "boba-fett"
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.Post.Json(vendorToAdd).ToUrl("/vendors");
            api.StatusCodeShouldBe(201);
        });

        var locationHeader = postResult.Context.Response.Headers.Location;
        var vendorReturnedFromPost = postResult.ReadAsJson<VendorCreateResponse>();
        Assert.NotNull(vendorReturnedFromPost);

        // Look at the vendor it returns
        // Get that vendor 

        var getResult = await fixture.Host.Scenario(api =>
        {
            api.Get.Url(locationHeader.ToString());
            api.StatusCodeShouldBeOk();
            // If they match, you are probably pretty good.
        });

        var vendorReturnedFromGet = getResult.ReadAsJson<VendorCreateResponse>();
        Assert.NotNull(vendorReturnedFromGet);
        Assert.Equal(vendorReturnedFromPost, vendorReturnedFromGet);

        // I can actually look in the database! Try doing THAT from Postman, or Playwright!

        using var sp = fixture.Host.Services.CreateScope(); // brand new "Scope" - like a request
        using var db = sp.ServiceProvider.GetRequiredService<IDocumentSession>(); // Have that create me a IDocumentSession, which is scoped to this "request"

        var savedVendor = await db.LoadAsync<VendorEntity>(vendorReturnedFromGet.Id, TestContext.Current.CancellationToken);

        Assert.NotNull(savedVendor);
        Assert.Equal("boba-fett", savedVendor.CreatedBy);


    }
}
