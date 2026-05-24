exec [dbo].[usp_practice_invitations_count]
    @practice_id = @practice_id,
    @status_name = @status_name,
    @role_name = @role_name,
    @search_text = @search_text;