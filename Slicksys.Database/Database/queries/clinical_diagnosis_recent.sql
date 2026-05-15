select top (200)
    d.[dx_guid],
    d.[patient_guid],
    d.[client_guid],
    d.[dx_date_time],
    d.[dx_code],
    d.[dx_description],
    d.[severity],
    d.[deleted],
    d.[create_date_time]
from [dbo].[mr_dx] as d
where d.[dx_date_time] >= @from_time
  and d.[dx_date_time] < @to_time
order by d.[dx_date_time] desc, d.[dx_guid] desc;