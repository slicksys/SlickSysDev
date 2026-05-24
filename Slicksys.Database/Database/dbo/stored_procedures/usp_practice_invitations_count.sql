create procedure [dbo].[usp_practice_invitations_count]
    @practice_id uniqueidentifier,
    @status_name nvarchar(20) = null,
    @role_name nvarchar(256) = null,
    @search_text nvarchar(256) = null
as
begin
    set nocount on;

    select
        count_big(1) as [total_rows]
    from [dbo].[user_practice_invitation] as upi
    inner join [dbo].[AspNetRoles] as r
        on r.[Id] = upi.[role_id]
    where upi.[practice_id] = @practice_id
      and (@status_name is null or upi.[status_name] = @status_name)
      and (@role_name is null or r.[Name] = @role_name or r.[NormalizedName] = upper(@role_name))
      and (
          @search_text is null
          or upi.[normalized_email] like N'%' + upper(@search_text) + N'%'
      );
end