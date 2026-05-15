create nonclustered index [ix_user_practice_invitation_practice_status]
    on [dbo].[user_practice_invitation] ([practice_id], [status_name])
    include ([normalized_email], [role_id], [expires_at], [created_at]);