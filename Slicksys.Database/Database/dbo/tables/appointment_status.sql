create table [dbo].[appointment_status] (
    [status_id] uniqueidentifier not null constraint [df_appointment_status_status_id] default (newid()),
    [practice_id] uniqueidentifier not null,
    [status_name] nvarchar(100) not null,
    [sort_order] int not null constraint [df_appointment_status_sort_order] default ((0)),
    [is_active] bit not null constraint [df_appointment_status_is_active] default ((1)),
    [color_code] nvarchar(20) null,
    constraint [pk_appointment_status] primary key clustered ([status_id]),
    constraint [fk_appointment_status_practice] foreign key ([practice_id]) references [dbo].[practice] ([practice_id])
);