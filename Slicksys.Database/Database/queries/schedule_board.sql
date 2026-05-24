select
    sb.[practice_id],
    sb.[item_type],
    sb.[appointment_id],
    sb.[reservation_id],
    sb.[client_id],
    sb.[principal_id],
    sb.[resource_id],
    sb.[resource_name],
    sb.[resource_type],
    sb.[status_id],
    sb.[status_name],
    sb.[start_time],
    sb.[end_time],
    sb.[comments],
    sb.[group_id],
    sb.[recurrence_id],
    sb.[is_deleted]
from [dbo].[schedule_board] as sb
where sb.[practice_id] = @practice_id
  and sb.[start_time] >= @from_time
  and sb.[start_time] < @to_time
order by sb.[start_time], sb.[end_time], sb.[item_type];