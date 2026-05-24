using System.ComponentModel.DataAnnotations;
using ManagementData.Api.Contracts.Common;

namespace ManagementData.Api.Contracts.PracticeAccess;

public sealed class PracticeUsersQuery
{
    [MaxLength(256)]
    public string? RoleName { get; init; }

    public bool ActiveOnly { get; init; } = true;

    [MaxLength(256)]
    public string? SearchText { get; init; }

    [Range(0, int.MaxValue)]
    public int OffsetRows { get; init; }

    [Range(1, 500)]
    public int FetchRows { get; init; } = 50;

    public bool SortDesc { get; init; }
}

public sealed class UserPracticesQuery
{
    [MaxLength(256)]
    public string? RoleName { get; init; }

    public bool ActiveOnly { get; init; } = true;

    [MaxLength(200)]
    public string? SearchText { get; init; }

    [Range(0, int.MaxValue)]
    public int OffsetRows { get; init; }

    [Range(1, 500)]
    public int FetchRows { get; init; } = 50;

    public bool SortDesc { get; init; }
}

public sealed class PracticeInvitationsQuery
{
    [MaxLength(20)]
    public string? StatusName { get; init; }

    [MaxLength(256)]
    public string? RoleName { get; init; }

    [MaxLength(256)]
    public string? SearchText { get; init; }

    [Range(0, int.MaxValue)]
    public int OffsetRows { get; init; }

    [Range(1, 500)]
    public int FetchRows { get; init; } = 50;

    public bool SortDesc { get; init; }
}

public sealed class AddUserPracticeRoleRequest
{
    [Required]
    [MaxLength(256)]
    public string RoleName { get; init; } = string.Empty;

    public DateTime? ExpiresAt { get; init; }
}

public sealed class CreatePracticeInvitationRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string RoleName { get; init; } = string.Empty;

    [MaxLength(450)]
    public string? InvitedByUserId { get; init; }

    public DateTime? ExpiresAt { get; init; }
}

public sealed class AcceptPracticeInvitationRequest
{
    [Required]
    [MaxLength(450)]
    public string AcceptedUserId { get; init; } = string.Empty;
}

public sealed class PracticeAuthorizationQuery
{
    [Required]
    [MaxLength(450)]
    public string UserId { get; init; } = string.Empty;

    [MaxLength(256)]
    public string? RequiredRoleName { get; init; }
}

public sealed class PracticeRoleAssignmentResponse
{
    public Guid UserPracticeRoleId { get; init; }
    public Guid PracticeId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string RoleId { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpiresAt { get; init; }
}

public sealed class PracticeUserResponse
{
    public Guid UserPracticeRoleId { get; init; }
    public Guid PracticeId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string RoleId { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public sealed class UserPracticeResponse
{
    public Guid UserPracticeRoleId { get; init; }
    public Guid PracticeId { get; init; }
    public string PracticeName { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string RoleId { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpiresAt { get; init; }
}

public sealed class PracticeInvitationResponse
{
    public Guid UserPracticeInvitationId { get; init; }
    public Guid PracticeId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string NormalizedEmail { get; init; } = string.Empty;
    public Guid InviteToken { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string RoleId { get; init; } = string.Empty;
    public string? InvitedByUserId { get; init; }
    public string? AcceptedUserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public DateTime? AcceptedAt { get; init; }
    public DateTime? RevokedAt { get; init; }
}

public sealed class PracticeAuthorizationResponse
{
    public Guid PracticeId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string? RequiredRoleName { get; init; }
    public bool IsAuthorized { get; init; }
}