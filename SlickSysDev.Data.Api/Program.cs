using Dapper;
using ManagementData.Api;
using ManagementData.Api.Data;
using ManagementData.Api.Features.Auth;
using ManagementData.Api.Features.PracticeAccess;
using ManagementData.Api.Features.Scheduling;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseApplicationPipeline();
app.MapApplicationEndpoints();
app.MapDefaultEndpoints();

app.Run();

public partial class Program;
