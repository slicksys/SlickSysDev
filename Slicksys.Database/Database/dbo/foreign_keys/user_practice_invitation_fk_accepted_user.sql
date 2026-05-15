alter table [dbo].[user_practice_invitation]
    add constraint [fk_user_practice_invitation_accepted_user]
    foreign key ([accepted_user_id]) references [dbo].[AspNetUsers] ([Id]);