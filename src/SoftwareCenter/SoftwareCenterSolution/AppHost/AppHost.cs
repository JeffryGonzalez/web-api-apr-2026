var builder = DistributedApplication.CreateBuilder(args);

var pg = builder.AddPostgres("pg-server")
    .WithLifetime(ContainerLifetime.Persistent);

var softwareDb = pg.AddDatabase("software-db"); // you have to create the database with the right schema, etc.

builder.AddProject<Projects.Software_Api>("software-api")
    .WithReference(softwareDb)
    .WaitFor(softwareDb);

builder.Build().Run();
