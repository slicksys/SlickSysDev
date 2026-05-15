exec [dbo].[usp_add_user_practice_role]
    @practice_id = @practice_id,
    @user_id = @user_id,
    @role_name = @role_name,
    @expires_at = @expires_at;