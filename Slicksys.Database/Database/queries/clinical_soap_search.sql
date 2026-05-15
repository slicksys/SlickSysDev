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
where s.[s_text] like N'%' + @search_term + N'%'
   or s.[o_text] like N'%' + @search_term + N'%'
   or s.[a_text] like N'%' + @search_term + N'%'
   or s.[p_text] like N'%' + @search_term + N'%'
order by s.[mr_date_time] desc, s.[soap_guid] desc;