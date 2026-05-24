update r
set
    r.[status_id] = coalesce(@status_id, r.[status_id]),
    r.[visual_status_id] = coalesce(@visual_status_id, r.[visual_status_id]),
    r.[arrival_date] = coalesce(@arrival_date, r.[arrival_date]),
    r.[ending_date] = coalesce(@ending_date, r.[ending_date]),
    r.[hospitalized_flag] = coalesce(@hospitalized_flag, r.[hospitalized_flag]),
    r.[comments] = coalesce(@comments, r.[comments]),
    r.[is_deleted] = coalesce(@is_deleted, r.[is_deleted])
output
    inserted.[reservation_id],
    inserted.[practice_id],
    inserted.[client_id],
    inserted.[principal_id],
    inserted.[arrival_date],
    inserted.[ending_date],
    inserted.[status_id],
    inserted.[visual_status_id],
    inserted.[hospitalized_flag],
    inserted.[comments],
    inserted.[source],
    inserted.[is_deleted],
    inserted.[created_at]
from [dbo].[reservation] as r
where r.[reservation_id] = @reservation_id
  and r.[practice_id] = @practice_id;