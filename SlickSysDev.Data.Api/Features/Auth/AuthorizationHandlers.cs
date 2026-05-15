using Microsoft.AspNetCore.Authorization;

namespace ManagementData.Api.Features.Auth;

public sealed class PracticeAccessAuthorizationHandler(
    IAuthRepository authRepository,
    IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<PracticeAccessRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PracticeAccessRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return;
        }

        var userId = GetCurrentUserId(context.User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        if (!TryGetRouteGuid(httpContext, "practiceId", out var practiceId))
        {
            return;
        }

        var isAuthorized = await authRepository.IsAuthorizedForPracticeAsync(
            practiceId,
            userId,
            requirement.RequiredRoleName,
            httpContext.RequestAborted);

        if (isAuthorized)
        {
            context.Succeed(requirement);
        }
    }

    private static string? GetCurrentUserId(ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
    }

    private static bool TryGetRouteGuid(HttpContext context, string key, out Guid value)
    {
        value = default;
        if (!context.Request.RouteValues.TryGetValue(key, out var routeValue) || routeValue is null)
        {
            return false;
        }

        return Guid.TryParse(routeValue.ToString(), out value);
    }
}

public sealed class CurrentUserRouteAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<CurrentUserRouteRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CurrentUserRouteRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }

        var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Task.CompletedTask;
        }

        if (!httpContext.Request.RouteValues.TryGetValue(requirement.RouteKey, out var routeValue) || routeValue is null)
        {
            return Task.CompletedTask;
        }

        if (string.Equals(currentUserId, routeValue.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public sealed class SelfOrPracticeAdminAuthorizationHandler(
    IAuthRepository authRepository,
    IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<SelfOrPracticeAdminRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfOrPracticeAdminRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return;
        }

        var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return;
        }

        var targetUserId = httpContext.Request.Query[requirement.QueryKey].ToString();
        if (string.IsNullOrWhiteSpace(targetUserId))
        {
            return;
        }

        if (string.Equals(currentUserId, targetUserId, StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
            return;
        }

        if (!httpContext.Request.RouteValues.TryGetValue(requirement.PracticeRouteKey, out var routeValue)
            || routeValue is null
            || !Guid.TryParse(routeValue.ToString(), out var practiceId))
        {
            return;
        }

        var isPracticeAdmin = await authRepository.IsAuthorizedForPracticeAsync(
            practiceId,
            currentUserId,
            "practice_admin",
            httpContext.RequestAborted);

        if (isPracticeAdmin)
        {
            context.Succeed(requirement);
        }
    }
}
