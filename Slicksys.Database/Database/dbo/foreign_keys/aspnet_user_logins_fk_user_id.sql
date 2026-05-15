alter table [dbo].[AspNetUserLogins]
    add constraint [FK_AspNetUserLogins_AspNetUsers_UserId]
    foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id])
    on delete cascade;