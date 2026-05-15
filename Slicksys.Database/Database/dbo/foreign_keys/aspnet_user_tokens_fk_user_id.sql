alter table [dbo].[AspNetUserTokens]
    add constraint [FK_AspNetUserTokens_AspNetUsers_UserId]
    foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id])
    on delete cascade;