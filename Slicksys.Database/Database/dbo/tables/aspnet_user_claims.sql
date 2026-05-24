create table [dbo].[AspNetUserClaims] (
    [Id] int not null identity(1,1),
    [UserId] nvarchar(450) not null,
    [ClaimType] nvarchar(max) null,
    [ClaimValue] nvarchar(max) null,
    constraint [PK_AspNetUserClaims] primary key clustered ([Id])
);