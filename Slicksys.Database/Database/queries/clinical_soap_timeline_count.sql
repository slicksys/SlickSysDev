select
    count_big(1) as [total_rows]
from [dbo].[mr_soap] as s
where s.[mr_date_time] >= @from_time
  and s.[mr_date_time] < @to_time;