alter table [dbo].[user_practice_role]
    add constraint [fk_user_practice_role_practice]
    foreign key ([practice_id]) references [dbo].[practice] ([practice_id])
    on delete cascade;