create table [dbo].[principal_context_label] (
    [context_label] nvarchar(50) not null,
    [display_name] nvarchar(100) not null,
    [is_active] bit not null constraint [df_principal_context_label_is_active] default ((1)),
    constraint [pk_principal_context_label] primary key clustered ([context_label])
);