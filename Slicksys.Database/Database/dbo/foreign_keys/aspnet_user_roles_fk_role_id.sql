alter table [dbo].[AspNetUserRoles]
    add constraint [FK_AspNetUserRoles_AspNetRoles_RoleId]
    foreign key ([RoleId]) references [dbo].[AspNetRoles] ([Id])
    on delete cascade;