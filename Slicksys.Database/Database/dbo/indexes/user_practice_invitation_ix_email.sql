create nonclustered index [ix_user_practice_invitation_email]
    on [dbo].[user_practice_invitation] ([normalized_email], [status_name])
    include ([practice_id], [role_id], [expires_at], [invite_token]);