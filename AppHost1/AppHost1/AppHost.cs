

var builder = DistributedApplication.CreateBuilder(args);


// Add a SQL Server database resource named "db"
var server = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

var db = server.AddDatabase("db");

// Inject the connection string into WebApplication1
builder.AddProject<Projects.WebApplication1>("webapplication1")
    .WaitFor(db)
    .WithReference(db);

builder.Build().Run();
