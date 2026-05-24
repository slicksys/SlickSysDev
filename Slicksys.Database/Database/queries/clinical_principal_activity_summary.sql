select
    x.[principal_id],
    count(case when x.[activity_type] = N'soap' then 1 end) as [soap_count],
    count(case when x.[activity_type] = N'note' then 1 end) as [note_count],
    count(case when x.[activity_type] = N'diagnosis' then 1 end) as [diagnosis_count],
    max(x.[activity_time]) as [last_activity_time]
from (
    select
        s.[soap_guid] as [activity_id],
        s.[soap_guid] as [principal_id],
        cast(N'soap' as nvarchar(20)) as [activity_type],
        s.[mr_date_time] as [activity_time]
    from [dbo].[mr_soap] as s
    where s.[mr_date_time] >= @from_time
      and s.[mr_date_time] < @to_time

    union all

    select
        n.[note_guid] as [activity_id],
        n.[patient_guid] as [principal_id],
        cast(N'note' as nvarchar(20)) as [activity_type],
        n.[note_date] as [activity_time]
    from [dbo].[mr_notes] as n
    where n.[note_date] >= @from_time
      and n.[note_date] < @to_time

    union all

    select
        d.[dx_guid] as [activity_id],
        d.[patient_guid] as [principal_id],
        cast(N'diagnosis' as nvarchar(20)) as [activity_type],
        d.[dx_date_time] as [activity_time]
    from [dbo].[mr_dx] as d
    where d.[dx_date_time] >= @from_time
      and d.[dx_date_time] < @to_time
) as x
where (@principal_id is null or x.[principal_id] = @principal_id)
group by x.[principal_id]
order by [last_activity_time] desc;