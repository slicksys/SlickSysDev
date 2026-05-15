insert into [dbo].[reservation] (
    [practice_id],
    [client_id],
    [principal_id],
    [arrival_date],
    [ending_date],
    [status_id],
    [visual_status_id],
    [hospitalized_flag],
    [comments],
    [source]
)
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
select
    @practice_id,
    @client_id,
    @principal_id,
    @arrival_date,
    @ending_date,
    @status_id,
    @visual_status_id,
    coalesce(@hospitalized_flag, 0),
    @comments,
    coalesce(@source, N'new');