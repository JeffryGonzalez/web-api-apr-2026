using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors;

[Collection("VendorsSystemTests")]
[Trait("Category", "Vendors")]
[Trait("Category", "System")]
public class GetAllVendors(VendorsFixture fixture)
{
    [Fact]
    public async Task GetVendorList()
    {
        var result = await fixture.Host.Scenario(api =>
        {
            api.Get.Url("/vendors");
            api.StatusCodeShouldBe(200);

        });

         var vendors = result.ReadAsJson<List<VendorSummaryItem>>();
         Assert.NotNull(vendors);
         // Assert.NotEmpty(vendors);
    }
}