exec [dbo].[usp_create_user_practice_invitation]
    @practice_id = @practice_id,
    @role_name = @role_name,
    @email = @email,
    @invited_by_user_id = @invited_by_user_id,
    @expires_at = @expires_at;