create table [dbo].[invoice] (
    [invoice_id] uniqueidentifier not null constraint [df_invoice_invoice_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [client_id] uniqueidentifier not null,
    [invoice_number] nvarchar(30) not null,
    [invoice_date] date not null,
    [due_date] date not null,
    [status_name] nvarchar(50) not null,
    [total_amount] decimal(18, 2) not null constraint [df_invoice_total_amount] default ((0)),
    [balance_amount] decimal(18, 2) not null constraint [df_invoice_balance_amount] default ((0)),
    [is_open] bit not null constraint [df_invoice_is_open] default ((1)),
    [created_at] datetime2(0) not null constraint [df_invoice_created_at] default (sysdatetime()),
    [source] nvarchar(50) not null constraint [df_invoice_source] default (N'new'),
    constraint [pk_invoice] primary key clustered ([invoice_id]),
    constraint [fk_invoice_practice] foreign key ([practice_id]) references [dbo].[practice] ([practice_id]),
    constraint [fk_invoice_client] foreign key ([client_id]) references [dbo].[client] ([client_id])
);