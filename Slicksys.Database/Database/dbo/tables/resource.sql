create table [dbo].[resource] (
    [resource_id] uniqueidentifier not null constraint [df_resource_resource_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [resource_name] nvarchar(200) not null,
    [resource_type] nvarchar(50) not null,
    [is_active] bit not null constraint [df_resource_is_active] default ((1)),
    [created_at] datetime2(0) not null constraint [df_resource_created_at] default (sysdatetime()),
    constraint [pk_resource] primary key clustered ([resource_id]),
    constraint [fk_resource_practice] foreign key ([practice_id]) references [dbo].[practice] ([practice_id])
);