create procedure [dbo].[usp_schedule_board_count]
    @practice_id uniqueidentifier,
    @from_time datetime2(0),
    @to_time datetime2(0),
    @client_id uniqueidentifier = null,
    @principal_id uniqueidentifier = null,
    @status_id uniqueidentifier = null,
    @item_type nvarchar(20) = null
as
begin
    set nocount on;

    select
        count_big(1) as [total_rows]
    from [dbo].[schedule_board] as sb
    where sb.[practice_id] = @practice_id
      and sb.[start_time] >= @from_time
      and sb.[start_time] < @to_time
      and (@client_id is null or sb.[client_id] = @client_id)
      and (@principal_id is null or sb.[principal_id] = @principal_id)
      and (@status_id is null or sb.[status_id] = @status_id)
      and (@item_type is null or sb.[item_type] = @item_type);
end