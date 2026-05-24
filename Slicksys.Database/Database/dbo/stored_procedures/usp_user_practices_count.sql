create procedure [dbo].[usp_user_practices_count]
    @user_id nvarchar(450),
    @role_name nvarchar(256) = null,
    @active_only bit = 1,
    @search_text nvarchar(200) = null
as
begin
    set nocount on;

    select
        count_big(1) as [total_rows]
    from [dbo].[user_practice_role] as upr
    inner join [dbo].[practice] as p
        on p.[practice_id] = upr.[practice_id]
        inner join [dbo].[AspNetRoles] as r
                on r.[Id] = upr.[role_id]
    where upr.[user_id] = @user_id
            and (@role_name is null or r.[Name] = @role_name or r.[NormalizedName] = upper(@role_name))
    and (@active_only = 0 or (upr.[is_active] = 1 and (upr.[expires_at] is null or upr.[expires_at] > sysdatetime())))
      and (
          @search_text is null
          or p.[practice_name] like N'%' + @search_text + N'%'
      );
end