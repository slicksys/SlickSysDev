create view [dbo].[schedule_board]
as
select
    a.[practice_id],
    a.[appointment_id],
    cast(null as uniqueidentifier) as [reservation_id],
    a.[client_id],
    a.[principal_id],
    a.[resource_id],
    r.[resource_name],
    r.[resource_type],
    a.[status_id],
    s.[status_name],
    a.[start_time],
    a.[end_time],
    a.[comments],
    a.[group_id],
    a.[recurrence_id],
    a.[is_deleted],
    cast(N'appointment' as nvarchar(20)) as [item_type]
from [dbo].[appointment] as a
left join [dbo].[resource] as r
    on r.[resource_id] = a.[resource_id]
left join [dbo].[appointment_status] as s
    on s.[status_id] = a.[status_id]
union all
select
    rsv.[practice_id],
    cast(null as uniqueidentifier) as [appointment_id],
    rsv.[reservation_id],
    rsv.[client_id],
    rsv.[principal_id],
    cast(null as uniqueidentifier) as [resource_id],
    cast(null as nvarchar(200)) as [resource_name],
    cast(null as nvarchar(50)) as [resource_type],
    rsv.[status_id],
    rs.[status_name],
    rsv.[arrival_date] as [start_time],
    rsv.[ending_date] as [end_time],
    rsv.[comments],
    cast(null as uniqueidentifier) as [group_id],
    cast(null as uniqueidentifier) as [recurrence_id],
    rsv.[is_deleted],
    cast(N'reservation' as nvarchar(20)) as [item_type]
from [dbo].[reservation] as rsv
left join [dbo].[reservation_status] as rs
    on rs.[status_id] = rsv.[status_id];