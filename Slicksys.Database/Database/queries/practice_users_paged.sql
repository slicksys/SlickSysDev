exec [dbo].[usp_practice_users_paged]
    @practice_id = @practice_id,
    @role_name = @role_name,
    @active_only = @active_only,
    @search_text = @search_text,
    @offset_rows = @offset_rows,
    @fetch_rows = @fetch_rows,
    @sort_desc = @sort_desc;