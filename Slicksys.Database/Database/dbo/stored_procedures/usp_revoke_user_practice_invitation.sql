create procedure [dbo].[usp_revoke_user_practice_invitation]
    @invite_token uniqueidentifier
as
begin
    set nocount on;

    update upi
    set
        upi.[status_name] = N'revoked',
        upi.[revoked_at] = sysdatetime()
    output
        inserted.[user_practice_invitation_id],
        inserted.[practice_id],
        inserted.[email],
        inserted.[normalized_email],
        inserted.[invite_token],
        inserted.[status_name],
        r.[Name] as [role_name],
        inserted.[role_id],
        inserted.[invited_by_user_id],
        inserted.[accepted_user_id],
        inserted.[created_at],
        inserted.[expires_at],
        inserted.[accepted_at],
        inserted.[revoked_at]
    from [dbo].[user_practice_invitation] as upi
    inner join [dbo].[AspNetRoles] as r
        on r.[Id] = upi.[role_id]
    where upi.[invite_token] = @invite_token
      and upi.[status_name] = N'pending';
end