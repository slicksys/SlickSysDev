create table [dbo].[resource_type] (
    [resource_type] nvarchar(50) not null,
    [display_name] nvarchar(100) not null,
    [is_active] bit not null constraint [df_resource_type_is_active] default ((1)),
    constraint [pk_resource_type] primary key clustered ([resource_type])
);