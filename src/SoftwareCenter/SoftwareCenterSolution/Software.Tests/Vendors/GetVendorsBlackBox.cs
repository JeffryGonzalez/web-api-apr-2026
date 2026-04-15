using Alba;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace Software.Tests.Vendors;

public class GetVendorsBlackBox
{
    [Fact(Skip ="Dumb - demo")]
    public async Task GetVendors()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:1338");

        var response = await client.GetAsync("/vendors");

        Assert.NotNull(response);
        Assert.Equal(200, (int)response.StatusCode);
    }
}

public class GetVendorsWithAlba
{
    [Fact]
    public async Task DoIt()
    {
        var host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("ConnectionStrings:software-db", "some connection string");
            config.ConfigureServices(services =>
            {
                // Use this to say "I don't have something yet, but use this for now"
                var x = 12;
            });
            config.ConfigureTestServices(services =>
            {
                // use this to secretly replace the coffee with folgers crystals.
            });
        });

        var response = await host.Scenario(api =>
        {
            api.Get.Url("/catalog");
            api.StatusCodeShouldBe(200);
        });

        Assert.NotNull(response);

        var items = response.ReadAsJson<List<string>>();
        Assert.NotNull(items);
        Assert.Contains("item1", items);
    }
}
