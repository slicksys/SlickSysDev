create view [dbo].[user_practice_access]
as
    select
        upr.[user_practice_role_id],
        upr.[practice_id],
        p.[practice_name],
        upr.[user_id],
        u.[UserName] as [user_name],
        u.[Email] as [email],
        upr.[role_id],
        r.[Name] as [role_name],
        r.[NormalizedName] as [role_normalized_name],
        upr.[is_active],
        upr.[created_at],
        upr.[expires_at],
        cast(
            case
                when upr.[is_active] = 1
                 and (upr.[expires_at] is null or upr.[expires_at] > sysdatetime())
                then 1
                else 0
            end as bit
        ) as [is_effective_active]
    from [dbo].[user_practice_role] as upr
    inner join [dbo].[practice] as p
        on p.[practice_id] = upr.[practice_id]
    inner join [dbo].[AspNetUsers] as u
        on u.[Id] = upr.[user_id]
    inner join [dbo].[AspNetRoles] as r
        on r.[Id] = upr.[role_id];