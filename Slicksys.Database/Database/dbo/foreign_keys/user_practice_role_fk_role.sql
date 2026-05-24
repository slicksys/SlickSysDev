alter table [dbo].[user_practice_role]
    add constraint [fk_user_practice_role_role]
    foreign key ([role_id]) references [dbo].[AspNetRoles] ([Id])
    on delete cascade;