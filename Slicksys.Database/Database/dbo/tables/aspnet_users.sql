create table [dbo].[AspNetUsers] (
    [Id] nvarchar(450) not null,
    [UserName] nvarchar(256) null,
    [NormalizedUserName] nvarchar(256) null,
    [Email] nvarchar(256) null,
    [NormalizedEmail] nvarchar(256) null,
    [EmailConfirmed] bit not null constraint [DF_AspNetUsers_EmailConfirmed] default ((0)),
    [PasswordHash] nvarchar(max) null,
    [SecurityStamp] nvarchar(max) null,
    [ConcurrencyStamp] nvarchar(max) null,
    [PhoneNumber] nvarchar(max) null,
    [PhoneNumberConfirmed] bit not null constraint [DF_AspNetUsers_PhoneNumberConfirmed] default ((0)),
    [TwoFactorEnabled] bit not null constraint [DF_AspNetUsers_TwoFactorEnabled] default ((0)),
    [LockoutEnd] datetimeoffset(7) null,
    [LockoutEnabled] bit not null constraint [DF_AspNetUsers_LockoutEnabled] default ((0)),
    [AccessFailedCount] int not null constraint [DF_AspNetUsers_AccessFailedCount] default ((0)),
    constraint [PK_AspNetUsers] primary key clustered ([Id])
);