if @sort_desc = 1
begin
    select
        n.[note_guid],
        n.[patient_guid],
        n.[client_guid],
        n.[note_date],
        n.[created_by],
        n.[note_text],
        n.[deleted],
        n.[create_date_time]
    from [dbo].[mr_notes] as n
    where n.[note_date] >= @from_time
      and n.[note_date] < @to_time
    order by n.[note_date] desc, n.[note_guid] desc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end
else
begin
    select
        n.[note_guid],
        n.[patient_guid],
        n.[client_guid],
        n.[note_date],
        n.[created_by],
        n.[note_text],
        n.[deleted],
        n.[create_date_time]
    from [dbo].[mr_notes] as n
    where n.[note_date] >= @from_time
      and n.[note_date] < @to_time
    order by n.[note_date] asc, n.[note_guid] asc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end