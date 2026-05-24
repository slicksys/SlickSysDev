create procedure [dbo].[usp_principal_summary_paged]
    @practice_id uniqueidentifier,
    @client_id uniqueidentifier = null,
    @principal_id uniqueidentifier = null,
    @context_label nvarchar(50) = null,
    @active bit = null,
    @offset_rows int = 0,
    @fetch_rows int = 50,
    @sort_desc bit = 0
as
begin
    set nocount on;

    if @sort_desc = 1
    begin
        select
            ps.[practice_id],
            ps.[principal_id],
            ps.[client_id],
            ps.[client_account_number],
            ps.[client_name],
            ps.[display_name],
            ps.[context_label],
            ps.[species],
            ps.[breed],
            ps.[sex],
            ps.[birthdate],
            ps.[active],
            ps.[latest_visit],
            ps.[flags],
            ps.[preventive_flags]
        from [dbo].[principal_summary] as ps
        where ps.[practice_id] = @practice_id
          and (@client_id is null or ps.[client_id] = @client_id)
          and (@principal_id is null or ps.[principal_id] = @principal_id)
          and (@context_label is null or ps.[context_label] = @context_label)
          and (@active is null or ps.[active] = @active)
        order by ps.[client_name] desc, ps.[display_name] desc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
    else
    begin
        select
            ps.[practice_id],
            ps.[principal_id],
            ps.[client_id],
            ps.[client_account_number],
            ps.[client_name],
            ps.[display_name],
            ps.[context_label],
            ps.[species],
            ps.[breed],
            ps.[sex],
            ps.[birthdate],
            ps.[active],
            ps.[latest_visit],
            ps.[flags],
            ps.[preventive_flags]
        from [dbo].[principal_summary] as ps
        where ps.[practice_id] = @practice_id
          and (@client_id is null or ps.[client_id] = @client_id)
          and (@principal_id is null or ps.[principal_id] = @principal_id)
          and (@context_label is null or ps.[context_label] = @context_label)
          and (@active is null or ps.[active] = @active)
        order by ps.[client_name] asc, ps.[display_name] asc
        offset @offset_rows rows fetch next @fetch_rows rows only;
    end
end