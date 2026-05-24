select top (50)
    p.[revision_guid],
    p.[prodm_guid],
    p.[code],
    p.[name],
    p.[barcode],
    p.[unit_of_measure],
    p.[decimals],
    p.[pkg_fee],
    p.[unit_price],
    p.[def_selling_price],
    p.[min_price],
    p.[def_qty],
    p.[special_discount_percent],
    p.[unit_cost],
    p.[comment],
    p.[vetinsite_guid],
    p.[dtype_guid],
    p.[milk],
    p.[meat],
    p.[create_date_time],
    p.[deleted]
from [dbo].[products_m] as p
where p.[code] = @code
order by p.[create_date_time] desc, p.[revision_guid] desc;