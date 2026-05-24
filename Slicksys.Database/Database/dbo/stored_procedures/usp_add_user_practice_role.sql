create procedure [dbo].[usp_add_user_practice_role]
    @practice_id uniqueidentifier,
    @user_id nvarchar(450),
    @role_name nvarchar(256),
    @expires_at datetime2(0) = null
as
begin
    set nocount on;

    declare @role_id nvarchar(450);

    select top 1
        @role_id = r.[Id]
    from [dbo].[AspNetRoles] as r
    where r.[Name] = @role_name
       or r.[NormalizedName] = upper(@role_name);

    if @role_id is null
    begin
        throw 51020, 'role_name must exist in AspNetRoles.', 1;
    end

    update [dbo].[user_practice_role]
    set
        [is_active] = 1,
        [expires_at] = @expires_at
    where [practice_id] = @practice_id
      and [user_id] = @user_id
      and [role_id] = @role_id;

    if @@rowcount = 0
    begin
        insert into [dbo].[user_practice_role] (
            [practice_id],
            [user_id],
            [role_id],
            [expires_at]
        )
        values (
            @practice_id,
            @user_id,
            @role_id,
            @expires_at
        );
    end

    select
        upr.[user_practice_role_id],
        upr.[practice_id],
        upr.[user_id],
        r.[Name] as [role_name],
        r.[Id] as [role_id],
        upr.[is_active],
        upr.[created_at],
        upr.[expires_at]
    from [dbo].[user_practice_role] as upr
    inner join [dbo].[AspNetRoles] as r
        on r.[Id] = upr.[role_id]
    where upr.[practice_id] = @practice_id
      and upr.[user_id] = @user_id
      and upr.[role_id] = @role_id;
end