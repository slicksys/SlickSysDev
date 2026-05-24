namespace ManagementData.Api.Features.Auth;

public sealed class AuthUserProfile
{
    public string UserId { get; init; } = string.Empty;
    public string? UserName { get; init; }
    public string? Email { get; init; }
}

public sealed class AuthTokenContext
{
    public required AuthUserProfile User { get; init; }
    public Guid? PracticeId { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
}