using Alba;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Software.Api.Vendors.Data;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Software.Tests.Vendors;

[Collection("VendorsUnitIntegrationTests")]
[Trait("Category", "Vendors")]
[Trait("Category", "System")]

public class AddingAVendor(VendorsUnitIntegrationTest fixture)
{

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
