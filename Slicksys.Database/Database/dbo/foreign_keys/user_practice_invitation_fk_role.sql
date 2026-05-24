alter table [dbo].[user_practice_invitation]
    add constraint [fk_user_practice_invitation_role]
    foreign key ([role_id]) references [dbo].[AspNetRoles] ([Id])
    on delete cascade;