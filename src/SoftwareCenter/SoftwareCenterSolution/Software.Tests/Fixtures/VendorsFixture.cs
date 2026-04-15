using Alba;
using Alba.Security;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Software.Api.Vendors.Services;
using Testcontainers.PostgreSql;

namespace Software.Tests.Fixtures;


[CollectionDefinition("VendorsSystemTests")]
public class VendorsSystemTestCollection : ICollectionFixture<VendorsFixture>;

public class VendorsFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; set; } = null!;
    private PostgreSqlContainer _pgContainer = null!;
    public async ValueTask InitializeAsync()
    {
        _pgContainer = new PostgreSqlBuilder("postgres:17.6")
            .Build();

        await _pgContainer.StartAsync();

        Host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("ConnectionStrings:software-db", _pgContainer.GetConnectionString());
            config.ConfigureServices(ConfigureServices);
            config.ConfigureTestServices(ConfigureTestServices);
            
        }, new AuthenticationStub());


    }

    protected virtual void ConfigureServices(IServiceCollection services) {}
    protected virtual void ConfigureTestServices(IServiceCollection services) {}
    
    public async ValueTask DisposeAsync()
    {
        await Host.DisposeAsync();
        await _pgContainer.DisposeAsync();
    }

   
}


public class VendorsUnitIntegrationTest : VendorsFixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        //var fakeUserThing = Substitute.For<IProvideTheCallingUser>();
        //fakeUserThing.GetSubClaim().Returns("boba-fett");
        //services.AddScoped<IProvideTheCallingUser>(_ => fakeUserThing);
    }
}


[CollectionDefinition("VendorsUnitIntegrationTests")]
public class VendorsUnitIntegrationTestCollection : ICollectionFixture<VendorsUnitIntegrationTest>;