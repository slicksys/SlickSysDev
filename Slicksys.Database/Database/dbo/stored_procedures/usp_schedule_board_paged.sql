create procedure [dbo].[usp_schedule_board_paged]
    @practice_id uniqueidentifier,
    @from_time datetime2(0),
    @to_time datetime2(0),
    @client_id uniqueidentifier = null,
    @principal_id uniqueidentifier = null,
    @status_id uniqueidentifier = null,
    @item_type nvarchar(20) = null,
    @offset_rows int = 0,
    @fetch_rows int = 50,
    @sort_desc bit = 0
as
begin
    set nocount on;

    if @sort_desc = 1
    begin
        select
            sb.[practice_id],
            sb.[item_type],
            sb.[appointment_id],
            sb.[reservation_id],
            sb.[client_id],
            sb.[principal_id],
            sb.[resource_id],
            sb.[resource_name],
            sb.[resource_type],
            sb.[status_id],
            sb.[status_name],
            sb.[start_time],
            sb.[end_time],
            sb.[comments],
            sb.[group_id],
            sb.[recurrence_id],
            sb.[is_deleted]
        from [dbo].[schedule_board] as sb
        where sb.[practice_id] = @practice_id
          and sb.[start_time] >= @from_time
          and sb.[start_time] < @to_time
          and (@client_id is null or sb.[client_id] = @client_id)
          and (@principal_id is null or sb.[principal_id] = @principal_id)
          and (@status_id is null or sb.[status_id] = @status_id)
          and (@item_type is null or sb.[item_type] = @item_type)
        order by sb.[start_time] desc, sb.[end_time] desc, sb.[item_type] desc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
    else
    begin
        select
            sb.[practice_id],
            sb.[item_type],
            sb.[appointment_id],
            sb.[reservation_id],
            sb.[client_id],
            sb.[principal_id],
            sb.[resource_id],
            sb.[resource_name],
            sb.[resource_type],
            sb.[status_id],
            sb.[status_name],
            sb.[start_time],
            sb.[end_time],
            sb.[comments],
            sb.[group_id],
            sb.[recurrence_id],
            sb.[is_deleted]
        from [dbo].[schedule_board] as sb
        where sb.[practice_id] = @practice_id
          and sb.[start_time] >= @from_time
          and sb.[start_time] < @to_time
          and (@client_id is null or sb.[client_id] = @client_id)
          and (@principal_id is null or sb.[principal_id] = @principal_id)
          and (@status_id is null or sb.[status_id] = @status_id)
          and (@item_type is null or sb.[item_type] = @item_type)
        order by sb.[start_time] asc, sb.[end_time] asc, sb.[item_type] asc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
end