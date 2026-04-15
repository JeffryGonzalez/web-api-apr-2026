using Marten;
using Software.Api;
using Software.Api.Catalog;
using Software.Api.Vendors.Models;
using Software.Api.Vendors.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddNpgsqlDataSource("software-db");
builder.AddServiceDefaults();
// Add services to the container.

//builder.AddNpgsqlDataSource("software-db");

// Need this for the source generated validation. 
// Really only has to do with minimal APIs
builder.Services.AddValidation();
builder.Services.AddBunny();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var connectionString = builder.Configuration.GetConnectionString("software-db") ?? throw new InvalidOperationException("Connection string 'software-db' not found.");
// In reality, by default it is going to look in ALL of the following places and use the last one it finds, or if none, null.
// - appsettings.json
// - appsettings.{Environment}.json
// - User Secrets (if in development) probably don't, check..
// - Environment Variables
// - Command Line Arguments

builder.Services.AddMarten(options =>
{
    // options.Connection(connectionString);
    // set up the appropriate singleton services to maintain the connection pool
    // and it will register a scoped service (IDocumentSession) that you can inject into your controllers

}).UseLightweightSessions()
.UseNpgsqlDataSource();

// Hey, Kestrel, if you create a controller and it needs a VendorData, you can create one of those for me, FOR EACH REQUEST,
// but only one per request.
builder.Services.AddScoped<IManageVendors, PostgresMartenVendorData>();

//builder.Services.AddSingleton<SomeType>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//app.MapPost("/test", (Vendor vendor) => Results.Ok(vendor));
app.MapCatalog();
app.MapControllers();
app.MapDefaultEndpoints();
app.Run();
