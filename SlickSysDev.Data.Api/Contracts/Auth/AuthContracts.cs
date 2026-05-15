using System.ComponentModel.DataAnnotations;

namespace ManagementData.Api.Contracts.Auth;

public sealed class TokenRequest
{
    [Required]
    [MaxLength(256)]
    public string UsernameOrEmail { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(200)]
    public string Password { get; init; } = string.Empty;

    public Guid? PracticeId { get; init; }

    [MaxLength(256)]
    public string? RequiredRoleName { get; init; }
}

public sealed class TokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string TokenType { get; init; } = "Bearer";
    public int ExpiresInSeconds { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Guid? PracticeId { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
}