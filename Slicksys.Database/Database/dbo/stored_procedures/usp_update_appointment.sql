create procedure [dbo].[usp_update_appointment]
    @practice_id uniqueidentifier,
    @appointment_id uniqueidentifier,
    @status_id uniqueidentifier = null,
    @resource_id uniqueidentifier = null,
    @start_time datetime2(0) = null,
    @end_time datetime2(0) = null,
    @comments nvarchar(2000) = null,
    @is_deleted bit = null
as
begin
    set nocount on;

    update a
    set
        a.[status_id] = coalesce(@status_id, a.[status_id]),
        a.[resource_id] = coalesce(@resource_id, a.[resource_id]),
        a.[start_time] = coalesce(@start_time, a.[start_time]),
        a.[end_time] = coalesce(@end_time, a.[end_time]),
        a.[comments] = coalesce(@comments, a.[comments]),
        a.[is_deleted] = coalesce(@is_deleted, a.[is_deleted])
    output
        inserted.[appointment_id],
        inserted.[practice_id],
        inserted.[client_id],
        inserted.[principal_id],
        inserted.[resource_id],
        inserted.[status_id],
        inserted.[start_time],
        inserted.[end_time],
        inserted.[comments],
        inserted.[group_id],
        inserted.[recurrence_id],
        inserted.[source],
        inserted.[is_deleted],
        inserted.[created_at]
    from [dbo].[appointment] as a
    where a.[practice_id] = @practice_id
      and a.[appointment_id] = @appointment_id;
end