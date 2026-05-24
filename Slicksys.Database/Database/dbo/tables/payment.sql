create table [dbo].[payment] (
    [payment_id] uniqueidentifier not null constraint [df_payment_payment_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [client_id] uniqueidentifier not null,
    [invoice_id] uniqueidentifier null,
    [payment_amount] decimal(18, 2) not null,
    [payment_date] datetime2(0) not null,
    [payment_method] nvarchar(50) not null,
    [reference_number] nvarchar(100) null,
    [memo] nvarchar(400) null,
    [source] nvarchar(50) not null constraint [df_payment_source] default (N'new'),
    [posted_at] datetime2(0) not null constraint [df_payment_posted_at] default (sysdatetime()),
    constraint [pk_payment] primary key clustered ([payment_id]),
    constraint [fk_payment_practice] foreign key ([practice_id]) references [dbo].[practice] ([practice_id]),
    constraint [fk_payment_client] foreign key ([client_id]) references [dbo].[client] ([client_id]),
    constraint [fk_payment_invoice] foreign key ([invoice_id]) references [dbo].[invoice] ([invoice_id])
);