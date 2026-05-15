create nonclustered index [ix_user_practice_role_practice_role]
    on [dbo].[user_practice_role] ([practice_id], [role_id])
    include ([user_id], [is_active], [expires_at]);