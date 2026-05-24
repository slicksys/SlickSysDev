create table [dbo].[user_practice_invitation] (
    [user_practice_invitation_id] uniqueidentifier not null constraint [df_user_practice_invitation_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [role_id] nvarchar(450) not null,
    [email] nvarchar(256) not null,
    [normalized_email] nvarchar(256) not null,
    [invite_token] uniqueidentifier not null constraint [df_user_practice_invitation_token] default (newid()),
    [status_name] nvarchar(20) not null constraint [df_user_practice_invitation_status] default (N'pending'),
    [invited_by_user_id] nvarchar(450) null,
    [accepted_user_id] nvarchar(450) null,
    [created_at] datetime2(0) not null constraint [df_user_practice_invitation_created_at] default (sysdatetime()),
    [expires_at] datetime2(0) null,
    [accepted_at] datetime2(0) null,
    [revoked_at] datetime2(0) null,
    constraint [pk_user_practice_invitation] primary key clustered ([user_practice_invitation_id]),
    constraint [ck_user_practice_invitation_status] check ([status_name] in (N'pending', N'accepted', N'revoked', N'expired'))
);