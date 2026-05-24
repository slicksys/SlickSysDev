exec [dbo].[usp_accept_user_practice_invitation]
    @invite_token = @invite_token,
    @accepted_user_id = @accepted_user_id;