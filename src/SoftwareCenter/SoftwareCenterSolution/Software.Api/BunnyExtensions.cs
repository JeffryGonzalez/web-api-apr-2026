namespace Software.Api;

public static class BunnyExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBunny()
        {
            //services.AddScoped<IBunnySession, BunnySession>();
            //services.AddSingleton<IBunnyDatabaseConnectionManager, BunnyDatabaseConnectionManager>();
            return services;
        }
    }
}
