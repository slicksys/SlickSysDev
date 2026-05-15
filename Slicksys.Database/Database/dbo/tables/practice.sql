create table [dbo].[practice] (
    [practice_id] uniqueidentifier not null constraint [df_practice_practice_id] default (newid()),
    [practice_name] nvarchar(200) not null,
    [is_active] bit not null constraint [df_practice_is_active] default ((1)),
    [created_at] datetime2(0) not null constraint [df_practice_created_at] default (sysdatetime()),
    constraint [pk_practice] primary key clustered ([practice_id])
);