create table [dbo].[AspNetRoles] (
    [Id] nvarchar(450) not null,
    [Name] nvarchar(256) null,
    [NormalizedName] nvarchar(256) null,
    [ConcurrencyStamp] nvarchar(max) null,
    constraint [PK_AspNetRoles] primary key clustered ([Id])
);