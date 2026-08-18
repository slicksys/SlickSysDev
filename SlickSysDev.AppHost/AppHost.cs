using Aspire.Hosting.Azure;
using Azure.Provisioning.AppContainers;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("aca")
    .WithDashboard(false);

// Redis output cache used by the web frontends.
var cache = builder.AddRedis("cache");

// SQL connection string for the ManagementData API (configure in AppHost user secrets:
// ConnectionStrings:ManagementData).
var managementDb = builder.AddConnectionString("ManagementData");
var managementDataJwtSigningKey = builder.AddParameter("managementdata-jwt-signing-key", secret: true);

// APIs
var apiService = builder.AddProject<Projects.SlickSysDev_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .PublishAsAzureContainerApp(KeepWarm);

var dataApi = builder.AddProject<Projects.ManagementData_Api>("dataapi")
    .WithReference(managementDb)
    .WithEnvironment("Jwt__SigningKey", managementDataJwtSigningKey)
    .WithHttpHealthCheck("/health")
    .PublishAsAzureContainerApp(KeepWarm);

// Web frontends
builder.AddProject<Projects.SlickSysDev_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .PublishAsAzureContainerApp(KeepWarm);

builder.AddProject<Projects.SlickSysDev_Admin>("admin")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(dataApi)
    .PublishAsAzureContainerApp(KeepWarm);

builder.AddProject<Projects.SlickSysDev_Public>("public")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .PublishAsAzureContainerApp(KeepWarm);

builder.Build().Run();

static void KeepWarm(AzureResourceInfrastructure _, ContainerApp app)
{
    app.Template.Scale ??= new ContainerAppScale();
    app.Template.Scale.MinReplicas = 1;
    app.Template.Scale.MaxReplicas = 3;
}
