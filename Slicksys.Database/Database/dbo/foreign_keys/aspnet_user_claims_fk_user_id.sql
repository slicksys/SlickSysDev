alter table [dbo].[AspNetUserClaims]
    add constraint [FK_AspNetUserClaims_AspNetUsers_UserId]
    foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id])
    on delete cascade;