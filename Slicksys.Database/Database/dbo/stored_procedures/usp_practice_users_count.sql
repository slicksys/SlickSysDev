create procedure [dbo].[usp_practice_users_count]
    @practice_id uniqueidentifier,
    @role_name nvarchar(256) = null,
    @active_only bit = 1,
    @search_text nvarchar(256) = null
as
begin
    set nocount on;

    select
        count_big(1) as [total_rows]
    from [dbo].[user_practice_role] as upr
    inner join [dbo].[AspNetUsers] as u
        on u.[Id] = upr.[user_id]
        inner join [dbo].[AspNetRoles] as r
                on r.[Id] = upr.[role_id]
    where upr.[practice_id] = @practice_id
            and (@role_name is null or r.[Name] = @role_name or r.[NormalizedName] = upper(@role_name))
    and (@active_only = 0 or (upr.[is_active] = 1 and (upr.[expires_at] is null or upr.[expires_at] > sysdatetime())))
      and (
          @search_text is null
          or u.[UserName] like N'%' + @search_text + N'%'
          or u.[Email] like N'%' + @search_text + N'%'
      );
end