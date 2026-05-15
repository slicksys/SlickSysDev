using System.Data;
using Dapper;
using ManagementData.Api.Contracts.PracticeAccess;
using ManagementData.Api.Data;
using Microsoft.AspNetCore.Identity;

namespace ManagementData.Api.Features.Auth;

public sealed class SqlAuthRepository(IDbConnectionFactory dbConnectionFactory) : IAuthRepository
{
    private readonly PasswordHasher<IdentityUser> _passwordHasher = new();

    public async Task<AuthUserProfile?> ValidateCredentialsAsync(string usernameOrEmail, string password, CancellationToken cancellationToken)
    {
        const string sql = """
            select
                u.[Id] as [UserId],
                u.[UserName],
                u.[Email],
                u.[PasswordHash],
                u.[LockoutEnd],
                u.[LockoutEnabled]
            from [dbo].[AspNetUsers] as u
            where u.[NormalizedUserName] = @normalized_login
               or u.[NormalizedEmail] = @normalized_login;
            """;

        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var candidate = await connection.QuerySingleOrDefaultAsync<AuthUserCredentialCandidate>(new CommandDefinition(
            sql,
            new { normalized_login = usernameOrEmail.Trim().ToUpperInvariant() },
            commandType: CommandType.Text,
            cancellationToken: cancellationToken));

        if (candidate is null || string.IsNullOrWhiteSpace(candidate.PasswordHash))
        {
            return null;
        }

        if (candidate.LockoutEnabled && candidate.LockoutEnd.HasValue && candidate.LockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            return null;
        }

        var identityUser = new IdentityUser
        {
            Id = candidate.UserId,
            UserName = candidate.UserName,
            Email = candidate.Email
        };

        var verification = _passwordHasher.VerifyHashedPassword(identityUser, candidate.PasswordHash, password);
        if (verification is PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new AuthUserProfile
        {
            UserId = candidate.UserId,
            UserName = candidate.UserName,
            Email = candidate.Email
        };
    }

    public async Task<IReadOnlyCollection<string>> GetPracticeRolesAsync(Guid practiceId, string userId, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var rows = await connection.QueryAsync<PracticeRoleName>(new CommandDefinition(
            "dbo.usp_user_practices_paged",
            new
            {
                user_id = userId,
                role_name = (string?)null,
                active_only = true,
                search_text = (string?)null,
                offset_rows = 0,
                fetch_rows = 100,
                sort_desc = false
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return rows.Where(row => row.PracticeId == practiceId)
            .Select(row => row.RoleName)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public async Task<bool> IsAuthorizedForPracticeAsync(Guid practiceId, string userId, string? requiredRoleName, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var response = await connection.QuerySingleAsync<PracticeAuthorizationResponse>(new CommandDefinition(
            "dbo.usp_authorize_user_practice",
            new
            {
                practice_id = practiceId,
                user_id = userId,
                required_role_name = requiredRoleName
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return response.IsAuthorized;
    }

    private sealed class PracticeRoleName
    {
        public Guid PracticeId { get; init; }
        public string RoleName { get; init; } = string.Empty;
    }

    private sealed class AuthUserCredentialCandidate
    {
        public string UserId { get; init; } = string.Empty;
        public string? UserName { get; init; }
        public string? Email { get; init; }
        public string? PasswordHash { get; init; }
        public DateTimeOffset? LockoutEnd { get; init; }
        public bool LockoutEnabled { get; init; }
    }
}