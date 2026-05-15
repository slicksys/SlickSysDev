create procedure [dbo].[usp_principal_summary_count]
    @practice_id uniqueidentifier,
    @client_id uniqueidentifier = null,
    @principal_id uniqueidentifier = null,
    @context_label nvarchar(50) = null,
    @active bit = null
as
begin
    set nocount on;

    select
        count_big(1) as [total_rows]
    from [dbo].[principal_summary] as ps
    where ps.[practice_id] = @practice_id
      and (@client_id is null or ps.[client_id] = @client_id)
      and (@principal_id is null or ps.[principal_id] = @principal_id)
      and (@context_label is null or ps.[context_label] = @context_label)
      and (@active is null or ps.[active] = @active);
end