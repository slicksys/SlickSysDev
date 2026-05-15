exec [dbo].[usp_user_practices_count]
    @user_id = @user_id,
    @role_name = @role_name,
    @active_only = @active_only,
    @search_text = @search_text;