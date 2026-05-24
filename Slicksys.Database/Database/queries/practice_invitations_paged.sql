exec [dbo].[usp_practice_invitations_paged]
    @practice_id = @practice_id,
    @status_name = @status_name,
    @role_name = @role_name,
    @search_text = @search_text,
    @offset_rows = @offset_rows,
    @fetch_rows = @fetch_rows,
    @sort_desc = @sort_desc;