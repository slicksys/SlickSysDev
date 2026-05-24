using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.PracticeAccess;

namespace ManagementData.Api.Features.PracticeAccess;

public interface IPracticeAccessRepository
{
    Task<PagedResponse<PracticeUserResponse>> GetPracticeUsersAsync(Guid practiceId, PracticeUsersQuery query, CancellationToken cancellationToken);
    Task<PagedResponse<UserPracticeResponse>> GetUserPracticesAsync(string userId, UserPracticesQuery query, CancellationToken cancellationToken);
    Task<PagedResponse<PracticeInvitationResponse>> GetPracticeInvitationsAsync(Guid practiceId, PracticeInvitationsQuery query, CancellationToken cancellationToken);
    Task<PracticeRoleAssignmentResponse> AddUserPracticeRoleAsync(Guid practiceId, string userId, AddUserPracticeRoleRequest request, CancellationToken cancellationToken);
    Task<PracticeRoleAssignmentResponse?> RemoveUserPracticeRoleAsync(Guid practiceId, string userId, string roleName, DateTime? expiresAt, CancellationToken cancellationToken);
    Task<PracticeInvitationResponse> CreatePracticeInvitationAsync(Guid practiceId, CreatePracticeInvitationRequest request, CancellationToken cancellationToken);
    Task<PracticeInvitationResponse> AcceptPracticeInvitationAsync(Guid inviteToken, AcceptPracticeInvitationRequest request, CancellationToken cancellationToken);
    Task<PracticeInvitationResponse?> RevokePracticeInvitationAsync(Guid inviteToken, CancellationToken cancellationToken);
    Task<PracticeAuthorizationResponse> AuthorizeUserPracticeAsync(Guid practiceId, PracticeAuthorizationQuery query, CancellationToken cancellationToken);
}