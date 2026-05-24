alter table [dbo].[user_practice_invitation]
    add constraint [fk_user_practice_invitation_invited_by_user]
    foreign key ([invited_by_user_id]) references [dbo].[AspNetUsers] ([Id]);