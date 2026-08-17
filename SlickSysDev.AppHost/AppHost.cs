var builder = DistributedApplication.CreateBuilder(args);

// Redis output cache used by the web frontends.
var cache = builder.AddRedis("cache");

// SQL connection string for the ManagementData API (configure in AppHost user secrets:
// ConnectionStrings:ManagementData).
var managementDb = builder.AddConnectionString("ManagementData");

// APIs
var apiService = builder.AddProject<Projects.SlickSysDev_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var dataApi = builder.AddProject<Projects.ManagementData_Api>("dataapi")
    .WithReference(managementDb)
    .WithHttpHealthCheck("/health");

// Web frontends
builder.AddProject<Projects.SlickSysDev_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.SlickSysDev_Admin>("admin")
    .WithExternalHttpEndpoints()
 .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(dataApi);

builder.AddProject<Projects.SlickSysDev_Public>("public")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
