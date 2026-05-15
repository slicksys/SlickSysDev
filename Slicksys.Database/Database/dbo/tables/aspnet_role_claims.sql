create table [dbo].[AspNetRoleClaims] (
    [Id] int not null identity(1,1),
    [RoleId] nvarchar(450) not null,
    [ClaimType] nvarchar(max) null,
    [ClaimValue] nvarchar(max) null,
    constraint [PK_AspNetRoleClaims] primary key clustered ([Id])
);