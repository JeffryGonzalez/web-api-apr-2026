using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var pg = builder.AddPostgres("pg-server")
    .WithLifetime(ContainerLifetime.Persistent);

var softwareDb = pg.AddDatabase("software-db"); // you have to create the database with the right schema, etc.

var scalar = builder.AddScalarApiReference(options =>
{
    options.PreferHttpsEndpoint = true;
    options.AllowSelfSignedCertificates = true;
});




var softwareApi = builder.AddProject<Projects.Software_Api>("software-api")
    .WithReference(softwareDb)
    .WaitFor(softwareDb)
    .WaitFor(scalar);


scalar.WithApiReference(softwareApi);

builder.Build().Run();
