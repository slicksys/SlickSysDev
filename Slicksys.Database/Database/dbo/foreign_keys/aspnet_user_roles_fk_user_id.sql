alter table [dbo].[AspNetUserRoles]
    add constraint [FK_AspNetUserRoles_AspNetUsers_UserId]
    foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id])
    on delete cascade;