create table [dbo].[invoice_status_lookup] (
    [invoice_status] nvarchar(50) not null,
    [display_name] nvarchar(100) not null,
    [is_active] bit not null constraint [df_invoice_status_lookup_is_active] default ((1)),
    constraint [pk_invoice_status_lookup] primary key clustered ([invoice_status])
);