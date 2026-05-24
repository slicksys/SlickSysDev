using Microsoft.AspNetCore.Authorization;

namespace ManagementData.Api.Features.Auth;

public sealed class PracticeAccessRequirement(string? requiredRoleName = null) : IAuthorizationRequirement
{
    public string? RequiredRoleName { get; } = requiredRoleName;
}

public sealed class CurrentUserRouteRequirement(string routeKey) : IAuthorizationRequirement
{
    public string RouteKey { get; } = routeKey;
}

public sealed class SelfOrPracticeAdminRequirement(string queryKey, string practiceRouteKey) : IAuthorizationRequirement
{
    public string QueryKey { get; } = queryKey;
    public string PracticeRouteKey { get; } = practiceRouteKey;
}
