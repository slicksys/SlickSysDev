select top (200)
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
order by n.[note_date] desc, n.[note_guid] desc;