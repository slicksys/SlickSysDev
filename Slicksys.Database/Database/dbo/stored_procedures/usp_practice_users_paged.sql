create procedure [dbo].[usp_practice_users_paged]
    @practice_id uniqueidentifier,
    @role_name nvarchar(256) = null,
    @active_only bit = 1,
    @search_text nvarchar(256) = null,
    @offset_rows int = 0,
    @fetch_rows int = 50,
    @sort_desc bit = 0
as
begin
    set nocount on;

    if @sort_desc = 1
    begin
        select
            upr.[user_practice_role_id],
            upr.[practice_id],
            upr.[user_id],
                        r.[Name] as [role_name],
                        upr.[role_id],
            upr.[is_active],
            upr.[created_at],
            upr.[expires_at],
            u.[UserName] as [user_name],
            u.[Email] as [email]
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
          )
        order by upr.[created_at] desc, upr.[user_id] desc, r.[Name] desc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
    else
    begin
        select
            upr.[user_practice_role_id],
            upr.[practice_id],
            upr.[user_id],
                        r.[Name] as [role_name],
                        upr.[role_id],
            upr.[is_active],
            upr.[created_at],
            upr.[expires_at],
            u.[UserName] as [user_name],
            u.[Email] as [email]
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
          )
        order by upr.[created_at] asc, upr.[user_id] asc, r.[Name] asc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
end