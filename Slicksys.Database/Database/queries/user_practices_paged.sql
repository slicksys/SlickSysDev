exec [dbo].[usp_user_practices_paged]
    @user_id = @user_id,
    @role_name = @role_name,
    @active_only = @active_only,
    @search_text = @search_text,
    @offset_rows = @offset_rows,
    @fetch_rows = @fetch_rows,
    @sort_desc = @sort_desc;