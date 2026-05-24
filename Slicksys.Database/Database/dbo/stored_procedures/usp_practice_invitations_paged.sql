create procedure [dbo].[usp_practice_invitations_paged]
    @practice_id uniqueidentifier,
    @status_name nvarchar(20) = null,
    @role_name nvarchar(256) = null,
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
          and (@status_name is null or upi.[status_name] = @status_name)
          and (@role_name is null or r.[Name] = @role_name or r.[NormalizedName] = upper(@role_name))
          and (
              @search_text is null
              or upi.[normalized_email] like N'%' + upper(@search_text) + N'%'
          )
        order by upi.[created_at] desc, upi.[email] desc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
    else
    begin
        select
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
          and (@status_name is null or upi.[status_name] = @status_name)
          and (@role_name is null or r.[Name] = @role_name or r.[NormalizedName] = upper(@role_name))
          and (
              @search_text is null
              or upi.[normalized_email] like N'%' + upper(@search_text) + N'%'
          )
        order by upi.[created_at] asc, upi.[email] asc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
end