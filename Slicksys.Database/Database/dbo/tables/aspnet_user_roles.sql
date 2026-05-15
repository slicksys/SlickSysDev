create table [dbo].[AspNetUserRoles] (
    [UserId] nvarchar(450) not null,
    [RoleId] nvarchar(450) not null,
    constraint [PK_AspNetUserRoles] primary key clustered ([UserId], [RoleId])
);