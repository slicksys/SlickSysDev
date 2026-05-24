alter table [dbo].[principal]
add constraint [fk_principal_context_label]
foreign key ([context_label]) references [dbo].[principal_context_label] ([context_label]);