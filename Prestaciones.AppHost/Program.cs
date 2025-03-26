var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("prestaciones");

var api = builder.AddProject<Projects.Prestaciones_Api>("api")
    .WaitFor(postgres)
    .WithReference(postgres);

builder.AddProject<Projects.Prestaciones_Web>("web")
    .WaitFor(api)
    .WithReference(api);

builder.Build().Run();
