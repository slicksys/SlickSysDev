using System.Text;
using ManagementData.Api.Data;
using ManagementData.Api.Features.Auth;
using ManagementData.Api.Features.PracticeAccess;
using ManagementData.Api.Features.Scheduling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ManagementData.Api;

public static class ServiceConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddControllers();
        services.AddOpenApi();
        services.AddHealthChecks();

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt settings are not configured.");

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddHttpContextAccessor();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.PracticeMember, policy =>
                policy.AddRequirements(new PracticeAccessRequirement()));
            options.AddPolicy(AuthorizationPolicies.PracticeUser, policy =>
                policy.AddRequirements(new PracticeAccessRequirement("practice_user")));
            options.AddPolicy(AuthorizationPolicies.PracticeAdmin, policy =>
                policy.AddRequirements(new PracticeAccessRequirement("practice_admin")));
            options.AddPolicy(AuthorizationPolicies.CurrentUserRoute, policy =>
                policy.AddRequirements(new CurrentUserRouteRequirement("userId")));
            options.AddPolicy(AuthorizationPolicies.SelfOrPracticeAdmin, policy =>
                policy.AddRequirements(new SelfOrPracticeAdminRequirement("userId", "practiceId")));
        });

        services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IAuthRepository, SqlAuthRepository>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPracticeAuthorizationService, PracticeAuthorizationService>();
        services.AddScoped<IAuthorizationHandler, PracticeAccessAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, CurrentUserRouteAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, SelfOrPracticeAdminAuthorizationHandler>();
        services.AddScoped<IPracticeAccessRepository, SqlPracticeAccessRepository>();
        services.AddScoped<ISchedulingRepository, SqlSchedulingRepository>();

        return services;
    }
}