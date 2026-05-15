if @sort_desc = 1
begin
    select
        *
    from [dbo].[labresults]
    order by 1 desc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end
else
begin
    select
        *
    from [dbo].[labresults]
    order by 1 asc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end