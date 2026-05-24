create procedure [dbo].[usp_accept_user_practice_invitation]
    @invite_token uniqueidentifier,
    @accepted_user_id nvarchar(450)
as
begin
    set nocount on;

    declare @practice_id uniqueidentifier;
    declare @role_id nvarchar(450);

    select top 1
        @practice_id = upi.[practice_id],
        @role_id = upi.[role_id]
    from [dbo].[user_practice_invitation] as upi
    where upi.[invite_token] = @invite_token
      and upi.[status_name] = N'pending'
      and (upi.[expires_at] is null or upi.[expires_at] > sysdatetime());

    if @practice_id is null or @role_id is null
    begin
        throw 51023, 'invitation token is invalid, expired, or not pending.', 1;
    end

    update [dbo].[user_practice_invitation]
    set
        [status_name] = N'accepted',
        [accepted_user_id] = @accepted_user_id,
        [accepted_at] = sysdatetime()
    where [invite_token] = @invite_token;

    update [dbo].[user_practice_role]
    set
        [is_active] = 1,
        [expires_at] = null
    where [practice_id] = @practice_id
      and [user_id] = @accepted_user_id
      and [role_id] = @role_id;

    if @@rowcount = 0
    begin
        insert into [dbo].[user_practice_role] (
            [practice_id],
            [user_id],
            [role_id],
            [is_active],
            [expires_at]
        )
        values (
            @practice_id,
            @accepted_user_id,
            @role_id,
            1,
            null
        );
    end

    select top 1
        upi.[user_practice_invitation_id],
        upi.[practice_id],
        upi.[email],
        upi.[normalized_email],
        upi.[invite_token],
        upi.[status_name],
        r.[Name] as [role_name],
        upi.[role_id],
        upi.[invited_by_user_id],
        upi.[accepted_user_id],
        upi.[created_at],
        upi.[expires_at],
        upi.[accepted_at],
        upi.[revoked_at]
    from [dbo].[user_practice_invitation] as upi
    inner join [dbo].[AspNetRoles] as r
        on r.[Id] = upi.[role_id]
    where upi.[invite_token] = @invite_token;
end