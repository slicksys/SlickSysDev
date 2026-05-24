create procedure [dbo].[usp_create_reservation]
    @practice_id uniqueidentifier,
    @client_id uniqueidentifier,
    @principal_id uniqueidentifier = null,
    @arrival_date datetime2(0),
    @ending_date datetime2(0),
    @status_id uniqueidentifier,
    @visual_status_id uniqueidentifier = null,
    @hospitalized_flag bit = 0,
    @comments nvarchar(2000) = null,
    @source nvarchar(50) = null
as
begin
    set nocount on;

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
    values (
        @practice_id,
        @client_id,
        @principal_id,
        @arrival_date,
        @ending_date,
        @status_id,
        @visual_status_id,
        @hospitalized_flag,
        @comments,
        coalesce(@source, N'new')
    );
end