create procedure [dbo].[usp_create_user_practice_invitation]
    @practice_id uniqueidentifier,
    @role_name nvarchar(256),
    @email nvarchar(256),
    @invited_by_user_id nvarchar(450) = null,
    @expires_at datetime2(0) = null
as
begin
    set nocount on;

    declare @role_id nvarchar(450);
    declare @normalized_email nvarchar(256) = upper(ltrim(rtrim(@email)));
    declare @invite_token uniqueidentifier = newid();

    select top 1
        @role_id = r.[Id]
    from [dbo].[AspNetRoles] as r
    where r.[Name] = @role_name
       or r.[NormalizedName] = upper(@role_name);

    if @role_id is null
    begin
        throw 51022, 'role_name must exist in AspNetRoles.', 1;
    end

    update [dbo].[user_practice_invitation]
    set
        [status_name] = N'pending',
        [invited_by_user_id] = @invited_by_user_id,
        [invite_token] = @invite_token,
        [expires_at] = @expires_at,
        [accepted_user_id] = null,
        [accepted_at] = null,
        [revoked_at] = null
    where [practice_id] = @practice_id
      and [normalized_email] = @normalized_email
      and [role_id] = @role_id
      and [status_name] in (N'pending', N'revoked', N'expired');

    if @@rowcount = 0
    begin
        insert into [dbo].[user_practice_invitation] (
            [practice_id],
            [role_id],
            [email],
            [normalized_email],
            [invite_token],
            [status_name],
            [invited_by_user_id],
            [expires_at]
        )
        values (
            @practice_id,
            @role_id,
            @email,
            @normalized_email,
            @invite_token,
            N'pending',
            @invited_by_user_id,
            @expires_at
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
    where upi.[practice_id] = @practice_id
      and upi.[normalized_email] = @normalized_email
      and upi.[role_id] = @role_id
    order by upi.[created_at] desc;
end