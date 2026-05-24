create procedure [dbo].[usp_billing_summary_count]
    @practice_id uniqueidentifier,
    @client_id uniqueidentifier = null,
    @billing_status nvarchar(50) = null
as
begin
    set nocount on;

    select
        count_big(1) as [total_rows]
    from [dbo].[billing_summary] as bs
    where bs.[practice_id] = @practice_id
      and (@client_id is null or bs.[client_id] = @client_id)
      and (@billing_status is null or bs.[billing_status] = @billing_status);
end