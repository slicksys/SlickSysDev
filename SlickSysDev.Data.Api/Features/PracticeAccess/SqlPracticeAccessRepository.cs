using System.Data;
using Dapper;
using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.PracticeAccess;
using ManagementData.Api.Data;

namespace ManagementData.Api.Features.PracticeAccess;

public sealed class SqlPracticeAccessRepository(IDbConnectionFactory dbConnectionFactory) : IPracticeAccessRepository
{
    public async Task<PagedResponse<PracticeUserResponse>> GetPracticeUsersAsync(Guid practiceId, PracticeUsersQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var parameters = new
        {
            practice_id = practiceId,
            role_name = query.RoleName,
            active_only = query.ActiveOnly,
            search_text = query.SearchText,
            offset_rows = query.OffsetRows,
            fetch_rows = query.FetchRows,
            sort_desc = query.SortDesc
        };

        var items = (await connection.QueryAsync<PracticeUserResponse>(new CommandDefinition(
            "dbo.usp_practice_users_paged",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken))).AsList();

        var totalRows = await connection.QuerySingleAsync<long>(new CommandDefinition(
            "dbo.usp_practice_users_count",
            new
            {
                practice_id = practiceId,
                role_name = query.RoleName,
                active_only = query.ActiveOnly,
                search_text = query.SearchText
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return new PagedResponse<PracticeUserResponse>(items, totalRows, query.OffsetRows, query.FetchRows);
    }

    public async Task<PagedResponse<UserPracticeResponse>> GetUserPracticesAsync(string userId, UserPracticesQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var parameters = new
        {
            user_id = userId,
            role_name = query.RoleName,
            active_only = query.ActiveOnly,
            search_text = query.SearchText,
            offset_rows = query.OffsetRows,
            fetch_rows = query.FetchRows,
            sort_desc = query.SortDesc
        };

        var items = (await connection.QueryAsync<UserPracticeResponse>(new CommandDefinition(
            "dbo.usp_user_practices_paged",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken))).AsList();

        var totalRows = await connection.QuerySingleAsync<long>(new CommandDefinition(
            "dbo.usp_user_practices_count",
            new
            {
                user_id = userId,
                role_name = query.RoleName,
                active_only = query.ActiveOnly,
                search_text = query.SearchText
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return new PagedResponse<UserPracticeResponse>(items, totalRows, query.OffsetRows, query.FetchRows);
    }

    public async Task<PagedResponse<PracticeInvitationResponse>> GetPracticeInvitationsAsync(Guid practiceId, PracticeInvitationsQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var parameters = new
        {
            practice_id = practiceId,
            status_name = query.StatusName,
            role_name = query.RoleName,
            search_text = query.SearchText,
            offset_rows = query.OffsetRows,
            fetch_rows = query.FetchRows,
            sort_desc = query.SortDesc
        };

        var items = (await connection.QueryAsync<PracticeInvitationResponse>(new CommandDefinition(
            "dbo.usp_practice_invitations_paged",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken))).AsList();

        var totalRows = await connection.QuerySingleAsync<long>(new CommandDefinition(
            "dbo.usp_practice_invitations_count",
            new
            {
                practice_id = practiceId,
                status_name = query.StatusName,
                role_name = query.RoleName,
                search_text = query.SearchText
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return new PagedResponse<PracticeInvitationResponse>(items, totalRows, query.OffsetRows, query.FetchRows);
    }

    public async Task<PracticeRoleAssignmentResponse> AddUserPracticeRoleAsync(Guid practiceId, string userId, AddUserPracticeRoleRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleAsync<PracticeRoleAssignmentResponse>(new CommandDefinition(
            "dbo.usp_add_user_practice_role",
            new
            {
                practice_id = practiceId,
                user_id = userId,
                role_name = request.RoleName,
                expires_at = request.ExpiresAt
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<PracticeRoleAssignmentResponse?> RemoveUserPracticeRoleAsync(Guid practiceId, string userId, string roleName, DateTime? expiresAt, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<PracticeRoleAssignmentResponse>(new CommandDefinition(
            "dbo.usp_remove_user_practice_role",
            new
            {
                practice_id = practiceId,
                user_id = userId,
                role_name = roleName,
                expires_at = expiresAt
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<PracticeInvitationResponse> CreatePracticeInvitationAsync(Guid practiceId, CreatePracticeInvitationRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleAsync<PracticeInvitationResponse>(new CommandDefinition(
            "dbo.usp_create_user_practice_invitation",
            new
            {
                practice_id = practiceId,
                role_name = request.RoleName,
                email = request.Email,
                invited_by_user_id = request.InvitedByUserId,
                expires_at = request.ExpiresAt
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<PracticeInvitationResponse> AcceptPracticeInvitationAsync(Guid inviteToken, AcceptPracticeInvitationRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleAsync<PracticeInvitationResponse>(new CommandDefinition(
            "dbo.usp_accept_user_practice_invitation",
            new
            {
                invite_token = inviteToken,
                accepted_user_id = request.AcceptedUserId
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<PracticeInvitationResponse?> RevokePracticeInvitationAsync(Guid inviteToken, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<PracticeInvitationResponse>(new CommandDefinition(
            "dbo.usp_revoke_user_practice_invitation",
            new { invite_token = inviteToken },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<PracticeAuthorizationResponse> AuthorizeUserPracticeAsync(Guid practiceId, PracticeAuthorizationQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleAsync<PracticeAuthorizationResponse>(new CommandDefinition(
            "dbo.usp_authorize_user_practice",
            new
            {
                practice_id = practiceId,
                user_id = query.UserId,
                required_role_name = query.RequiredRoleName
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }
}