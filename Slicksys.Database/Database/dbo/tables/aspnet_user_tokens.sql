create table [dbo].[AspNetUserTokens] (
    [UserId] nvarchar(450) not null,
    [LoginProvider] nvarchar(450) not null,
    [Name] nvarchar(450) not null,
    [Value] nvarchar(max) null,
    constraint [PK_AspNetUserTokens] primary key clustered ([UserId], [LoginProvider], [Name])
);