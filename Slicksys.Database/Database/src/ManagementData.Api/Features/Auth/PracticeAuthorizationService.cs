using System.Security.Claims;

namespace ManagementData.Api.Features.Auth;

public sealed class PracticeAuthorizationService(IAuthRepository authRepository) : IPracticeAuthorizationService
{
    public string? GetCurrentUserId(ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
    }

    public async Task<bool> IsAuthorizedAsync(ClaimsPrincipal principal, Guid practiceId, string? requiredRoleName, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId(principal);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        return await authRepository.IsAuthorizedForPracticeAsync(practiceId, userId, requiredRoleName, cancellationToken);
    }
}