

var builder = DistributedApplication.CreateBuilder(args);

// Add a PostgreSQL database resource named "db"
var db = builder.AddPostgres("db");

// Inject the connection string into WebApplication1
builder.AddProject<Projects.WebApplication1>("webapplication1")
    .WaitFor(db)
    .WithReference(db);

builder.Build().Run();
