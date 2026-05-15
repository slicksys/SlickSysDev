create table [dbo].[principal] (
    [principal_id] uniqueidentifier not null constraint [df_principal_principal_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [client_id] uniqueidentifier not null,
    [display_name] nvarchar(200) not null,
    [context_label] nvarchar(50) not null,
    [species] nvarchar(100) null,
    [breed] nvarchar(100) null,
    [sex] nvarchar(20) null,
    [birthdate] date null,
    [active] bit not null constraint [df_principal_active] default ((1)),
    [latest_visit] datetime2(0) null,
    [flags] nvarchar(max) null,
    [preventive_flags] nvarchar(max) null,
    [created_at] datetime2(0) not null constraint [df_principal_created_at] default (sysdatetime()),
    constraint [pk_principal] primary key clustered ([principal_id]),
    constraint [fk_principal_practice] foreign key ([practice_id]) references [dbo].[practice] ([practice_id]),
    constraint [fk_principal_client] foreign key ([client_id]) references [dbo].[client] ([client_id])
);