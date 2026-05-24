create table [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(450) not null,
    [ProviderKey] nvarchar(450) not null,
    [ProviderDisplayName] nvarchar(max) null,
    [UserId] nvarchar(450) not null,
    constraint [PK_AspNetUserLogins] primary key clustered ([LoginProvider], [ProviderKey])
);