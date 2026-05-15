create table [dbo].[client] (
    [client_id] uniqueidentifier not null constraint [df_client_client_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [client_account_number] nvarchar(20) not null,
    [client_name] nvarchar(200) not null,
    [billing_status] nvarchar(50) null,
    [credit_limit] decimal(18, 2) not null constraint [df_client_credit_limit] default ((0)),
    [is_active] bit not null constraint [df_client_is_active] default ((1)),
    [created_at] datetime2(0) not null constraint [df_client_created_at] default (sysdatetime()),
    constraint [pk_client] primary key clustered ([client_id]),
    constraint [fk_client_practice] foreign key ([practice_id]) references [dbo].[practice] ([practice_id])
);