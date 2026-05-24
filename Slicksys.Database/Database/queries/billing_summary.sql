select
    bs.[practice_id],
    bs.[client_id],
    bs.[client_name],
    bs.[ar_current],
    bs.[ar_30],
    bs.[ar_60],
    bs.[ar_90],
    bs.[total_due],
    bs.[credit_limit],
    bs.[last_pay_date],
    bs.[last_pay_amt],
    bs.[open_invoice_count],
    bs.[recent_invoice_count],
    bs.[billing_status]
from [dbo].[billing_summary] as bs
where bs.[practice_id] = @practice_id
  and (@client_id is null or bs.[client_id] = @client_id)
order by bs.[billing_status], bs.[client_name];