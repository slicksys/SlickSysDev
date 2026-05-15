alter table [dbo].[payment]
add constraint [fk_payment_payment_method]
foreign key ([payment_method]) references [dbo].[payment_method] ([payment_method]);