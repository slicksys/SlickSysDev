create procedure [dbo].[usp_authorize_user_practice]
    @practice_id uniqueidentifier,
    @user_id nvarchar(450),
    @required_role_name nvarchar(256) = null
as
begin
    set nocount on;

    declare @is_authorized bit = 0;

    if exists (
        select 1
        from [dbo].[user_practice_access] as upa
        where upa.[practice_id] = @practice_id
          and upa.[user_id] = @user_id
          and upa.[is_effective_active] = 1
          and (
              @required_role_name is null
              or upa.[role_name] = @required_role_name
              or upa.[role_normalized_name] = upper(@required_role_name)
          )
    )
    begin
        set @is_authorized = 1;
    end

    select
        @practice_id as [practice_id],
        @user_id as [user_id],
        @required_role_name as [required_role_name],
        @is_authorized as [is_authorized];
end