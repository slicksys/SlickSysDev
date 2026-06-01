var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");


var apiService = builder.AddProject<Projects.SlickSysDev_Data_Service > ("data-service")
        .WithHttpHealthCheck("/health");


var dataApi = builder.AddProject<Projects.SlickSysDev_DataApi>("edge-api")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.SlickSysDev_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.SlickSysDev_Public>("public")
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
    .WaitFor(apiService);


builder.Build().Run();
