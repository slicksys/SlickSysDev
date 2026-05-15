create view [dbo].[billing_summary]
as
with invoice_aging as (
    select
        i.[client_id],
        sum(case when datediff(day, i.[due_date], cast(sysdatetime() as date)) <= 0 then i.[balance_amount] else 0 end) as [ar_current],
        sum(case when datediff(day, i.[due_date], cast(sysdatetime() as date)) between 1 and 30 then i.[balance_amount] else 0 end) as [ar_30],
        sum(case when datediff(day, i.[due_date], cast(sysdatetime() as date)) between 31 and 60 then i.[balance_amount] else 0 end) as [ar_60],
        sum(case when datediff(day, i.[due_date], cast(sysdatetime() as date)) > 60 then i.[balance_amount] else 0 end) as [ar_90],
        sum(case when i.[is_open] = 1 then i.[balance_amount] else 0 end) as [total_due],
        sum(case when i.[invoice_date] >= dateadd(day, -30, cast(sysdatetime() as date)) then 1 else 0 end) as [recent_invoice_count],
        sum(case when i.[is_open] = 1 then 1 else 0 end) as [open_invoice_count]
    from [dbo].[invoice] as i
    group by i.[client_id]
)
select
    c.[practice_id],
    c.[client_id],
    c.[client_name],
    coalesce(ia.[ar_current], 0) as [ar_current],
    coalesce(ia.[ar_30], 0) as [ar_30],
    coalesce(ia.[ar_60], 0) as [ar_60],
    coalesce(ia.[ar_90], 0) as [ar_90],
    coalesce(ia.[total_due], 0) as [total_due],
    c.[credit_limit],
    lp.[last_pay_date],
    lp.[last_pay_amt],
    coalesce(ia.[open_invoice_count], 0) as [open_invoice_count],
    coalesce(ia.[recent_invoice_count], 0) as [recent_invoice_count],
    case
        when coalesce(ia.[total_due], 0) = 0 then N'clear'
        when coalesce(ia.[total_due], 0) <= c.[credit_limit] then N'watch'
        else N'past_due'
    end as [billing_status]
from [dbo].[client] as c
left join invoice_aging as ia
    on ia.[client_id] = c.[client_id]
outer apply (
    select top (1)
        p.[payment_date] as [last_pay_date],
        p.[payment_amount] as [last_pay_amt]
    from [dbo].[payment] as p
    where p.[client_id] = c.[client_id]
    order by p.[payment_date] desc, p.[payment_id] desc
) as lp;