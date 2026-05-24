create procedure [dbo].[usp_remove_user_practice_role]
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
        throw 51021, 'role_name must exist in AspNetRoles.', 1;
    end

    update upr
    set
        upr.[is_active] = 0,
        upr.[expires_at] = coalesce(@expires_at, upr.[expires_at], sysdatetime())
    output
        inserted.[user_practice_role_id],
        inserted.[practice_id],
        inserted.[user_id],
        r.[Name] as [role_name],
        inserted.[role_id],
        inserted.[is_active],
        inserted.[created_at],
        inserted.[expires_at]
    from [dbo].[user_practice_role] as upr
    inner join [dbo].[AspNetRoles] as r
        on r.[Id] = upr.[role_id]
    where upr.[practice_id] = @practice_id
      and upr.[user_id] = @user_id
      and upr.[role_id] = @role_id;
end