alter table [dbo].[AspNetRoleClaims]
    add constraint [FK_AspNetRoleClaims_AspNetRoles_RoleId]
    foreign key ([RoleId]) references [dbo].[AspNetRoles] ([Id])
    on delete cascade;