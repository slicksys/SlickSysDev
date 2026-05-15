namespace ManagementData.Api.Features.Auth;

public interface IPracticeAuthorizationService
{
    Task<bool> IsAuthorizedAsync(ClaimsPrincipal principal, Guid practiceId, string? requiredRoleName, CancellationToken cancellationToken);
    string? GetCurrentUserId(ClaimsPrincipal principal);
}