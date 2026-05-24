select
    count_big(1) as [total_rows]
from [dbo].[db_products] as p
where (@search_term is null)
   or p.[name] like N'%' + @search_term + N'%'
   or p.[code] like N'%' + @search_term + N'%'
   or p.[barcode] like N'%' + @search_term + N'%';