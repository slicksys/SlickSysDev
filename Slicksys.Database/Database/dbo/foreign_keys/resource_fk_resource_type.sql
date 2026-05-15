alter table [dbo].[resource]
add constraint [fk_resource_resource_type]
foreign key ([resource_type]) references [dbo].[resource_type] ([resource_type]);