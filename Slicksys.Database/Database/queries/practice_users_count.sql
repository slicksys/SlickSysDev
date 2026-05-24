exec [dbo].[usp_practice_users_count]
    @practice_id = @practice_id,
    @role_name = @role_name,
    @active_only = @active_only,
    @search_text = @search_text;