alter table [dbo].[invoice]
add constraint [fk_invoice_invoice_status]
foreign key ([status_name]) references [dbo].[invoice_status_lookup] ([invoice_status]);