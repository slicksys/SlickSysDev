namespace ManagementData.Api.Features.Auth;

public interface IAuthRepository
{
    Task<AuthUserProfile?> ValidateCredentialsAsync(string usernameOrEmail, string password, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<string>> GetPracticeRolesAsync(Guid practiceId, string userId, CancellationToken cancellationToken);
    Task<bool> IsAuthorizedForPracticeAsync(Guid practiceId, string userId, string? requiredRoleName, CancellationToken cancellationToken);
}