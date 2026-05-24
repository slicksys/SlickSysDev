using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.PracticeAccess;
using ManagementData.Api.Features.Auth;
using ManagementData.Api.Features.PracticeAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementData.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1")]
public sealed class PracticeAccessController(IPracticeAccessRepository repository) : ControllerBase
{
    [HttpGet("practices/{practiceId:guid}/users")]
    [Authorize(Policy = AuthorizationPolicies.PracticeMember)]
    [ProducesResponseType(typeof(PagedResponse<PracticeUserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<PracticeUserResponse>>> GetPracticeUsers(
        Guid practiceId,
        [FromQuery] PracticeUsersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetPracticeUsersAsync(practiceId, query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("users/{userId}/practices")]
    [Authorize(Policy = AuthorizationPolicies.CurrentUserRoute)]
    [ProducesResponseType(typeof(PagedResponse<UserPracticeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<UserPracticeResponse>>> GetUserPractices(
        string userId,
        [FromQuery] UserPracticesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetUserPracticesAsync(userId, query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("practices/{practiceId:guid}/invitations")]
    [Authorize(Policy = AuthorizationPolicies.PracticeAdmin)]
    [ProducesResponseType(typeof(PagedResponse<PracticeInvitationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<PracticeInvitationResponse>>> GetPracticeInvitations(
        Guid practiceId,
        [FromQuery] PracticeInvitationsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetPracticeInvitationsAsync(practiceId, query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("practices/{practiceId:guid}/authorization")]
    [Authorize(Policy = AuthorizationPolicies.SelfOrPracticeAdmin)]
    [ProducesResponseType(typeof(PracticeAuthorizationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PracticeAuthorizationResponse>> AuthorizeUserPractice(
        Guid practiceId,
        [FromQuery] PracticeAuthorizationQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.AuthorizeUserPracticeAsync(practiceId, query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("practices/{practiceId:guid}/users/{userId}/roles")]
    [Authorize(Policy = AuthorizationPolicies.PracticeAdmin)]
    [ProducesResponseType(typeof(PracticeRoleAssignmentResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PracticeRoleAssignmentResponse>> AddUserPracticeRole(
        Guid practiceId,
        string userId,
        [FromBody] AddUserPracticeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.AddUserPracticeRoleAsync(practiceId, userId, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("practices/{practiceId:guid}/users/{userId}/roles/{roleName}")]
    [Authorize(Policy = AuthorizationPolicies.PracticeAdmin)]
    [ProducesResponseType(typeof(PracticeRoleAssignmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PracticeRoleAssignmentResponse>> RemoveUserPracticeRole(
        Guid practiceId,
        string userId,
        string roleName,
        [FromQuery] DateTime? expiresAt,
        CancellationToken cancellationToken)
    {
        var result = await repository.RemoveUserPracticeRoleAsync(practiceId, userId, roleName, expiresAt, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("practices/{practiceId:guid}/invitations")]
    [Authorize(Policy = AuthorizationPolicies.PracticeAdmin)]
    [ProducesResponseType(typeof(PracticeInvitationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PracticeInvitationResponse>> CreatePracticeInvitation(
        Guid practiceId,
        [FromBody] CreatePracticeInvitationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.CreatePracticeInvitationAsync(practiceId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("invitations/{inviteToken:guid}/accept")]
    [Authorize]
    [ProducesResponseType(typeof(PracticeInvitationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PracticeInvitationResponse>> AcceptPracticeInvitation(
        Guid inviteToken,
        CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Forbid();
        }

        var request = new AcceptPracticeInvitationRequest
        {
            AcceptedUserId = currentUserId
        };

        var result = await repository.AcceptPracticeInvitationAsync(inviteToken, request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("practices/{practiceId:guid}/invitations/{inviteToken:guid}/revoke")]
    [Authorize(Policy = AuthorizationPolicies.PracticeAdmin)]
    [ProducesResponseType(typeof(PracticeInvitationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PracticeInvitationResponse>> RevokePracticeInvitation(
        Guid practiceId,
        Guid inviteToken,
        CancellationToken cancellationToken)
    {
        var result = await repository.RevokePracticeInvitationAsync(inviteToken, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}