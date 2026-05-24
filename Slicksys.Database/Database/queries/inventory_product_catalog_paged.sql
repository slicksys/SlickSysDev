if @sort_desc = 1
begin
    select
        p.[code],
        p.[name],
        p.[barcode],
        p.[unit_of_measure],
        p.[decimals],
        p.[unit_price],
        p.[def_selling_price],
        p.[min_price],
        p.[def_qty],
        p.[special_discount_percent],
        p.[unit_cost]
    from [dbo].[db_products] as p
    where (@search_term is null)
       or p.[name] like N'%' + @search_term + N'%'
       or p.[code] like N'%' + @search_term + N'%'
       or p.[barcode] like N'%' + @search_term + N'%'
    order by p.[name] desc, p.[code] desc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end
else
begin
    select
        p.[code],
        p.[name],
        p.[barcode],
        p.[unit_of_measure],
        p.[decimals],
        p.[unit_price],
        p.[def_selling_price],
        p.[min_price],
        p.[def_qty],
        p.[special_discount_percent],
        p.[unit_cost]
    from [dbo].[db_products] as p
    where (@search_term is null)
       or p.[name] like N'%' + @search_term + N'%'
       or p.[code] like N'%' + @search_term + N'%'
       or p.[barcode] like N'%' + @search_term + N'%'
    order by p.[name] asc, p.[code] asc
    offset @offset_rows rows fetch next @fetch_rows rows only;
end