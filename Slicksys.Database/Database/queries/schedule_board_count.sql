select
    count_big(1) as [total_rows]
from [dbo].[schedule_board] as sb
where sb.[practice_id] = @practice_id
  and sb.[start_time] >= @from_time
  and sb.[start_time] < @to_time
  and (@client_id is null or sb.[client_id] = @client_id)
  and (@principal_id is null or sb.[principal_id] = @principal_id)
  and (@status_id is null or sb.[status_id] = @status_id)
  and (@item_type is null or sb.[item_type] = @item_type);