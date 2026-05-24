exec [dbo].[usp_authorize_user_practice]
    @practice_id = @practice_id,
    @user_id = @user_id,
    @required_role_name = @required_role_name;