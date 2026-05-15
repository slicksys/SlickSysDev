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
order by ps.[client_name], ps.[display_name];