select top (200)
    s.[soap_guid],
    s.[mr_date_time],
    s.[s_text],
    s.[o_text],
    s.[a_text],
    s.[p_text],
    s.[deleted],
    s.[create_date_time]
from [dbo].[mr_soap] as s
where s.[mr_date_time] >= @from_time
  and s.[mr_date_time] < @to_time
order by s.[mr_date_time] desc, s.[soap_guid] desc;