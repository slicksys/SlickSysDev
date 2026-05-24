create table [dbo].[user_practice_role] (
    [user_practice_role_id] uniqueidentifier not null constraint [df_user_practice_role_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [user_id] nvarchar(450) not null,
    [role_id] nvarchar(450) not null,
    [is_active] bit not null constraint [df_user_practice_role_is_active] default ((1)),
    [created_at] datetime2(0) not null constraint [df_user_practice_role_created_at] default (sysdatetime()),
    [expires_at] datetime2(0) null,
    constraint [pk_user_practice_role] primary key clustered ([user_practice_role_id]),
    constraint [uq_user_practice_role_practice_user_role] unique nonclustered ([practice_id], [user_id], [role_id])
);