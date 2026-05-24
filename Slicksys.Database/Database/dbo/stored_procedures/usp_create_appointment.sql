create procedure [dbo].[usp_create_appointment]
    @practice_id uniqueidentifier,
    @client_id uniqueidentifier,
    @principal_id uniqueidentifier = null,
    @resource_id uniqueidentifier = null,
    @status_id uniqueidentifier,
    @start_time datetime2(0),
    @end_time datetime2(0),
    @comments nvarchar(2000) = null,
    @group_id uniqueidentifier = null,
    @recurrence_id uniqueidentifier = null,
    @source nvarchar(50) = null
as
begin
    set nocount on;

    insert into [dbo].[appointment] (
        [practice_id],
        [client_id],
        [principal_id],
        [resource_id],
        [status_id],
        [start_time],
        [end_time],
        [comments],
        [group_id],
        [recurrence_id],
        [source]
    )
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
    values (
        @practice_id,
        @client_id,
        @principal_id,
        @resource_id,
        @status_id,
        @start_time,
        @end_time,
        @comments,
        @group_id,
        @recurrence_id,
        coalesce(@source, N'new')
    );
end