alter table [dbo].[user_practice_role]
    add constraint [fk_user_practice_role_user]
    foreign key ([user_id]) references [dbo].[AspNetUsers] ([Id])
    on delete cascade;