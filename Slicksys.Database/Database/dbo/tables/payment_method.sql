create table [dbo].[payment_method] (
    [payment_method] nvarchar(50) not null,
    [display_name] nvarchar(100) not null,
    [is_active] bit not null constraint [df_payment_method_is_active] default ((1)),
    constraint [pk_payment_method] primary key clustered ([payment_method])
);