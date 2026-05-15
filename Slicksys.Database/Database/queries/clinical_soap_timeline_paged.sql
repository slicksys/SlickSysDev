if @sort_desc = 1
begin
    select
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
    order by s.[mr_date_time] desc, s.[soap_guid] desc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end
else
begin
    select
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
    order by s.[mr_date_time] asc, s.[soap_guid] asc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end