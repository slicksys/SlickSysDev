create view [dbo].[principal_summary]
as
select
    p.[practice_id],
    p.[principal_id],
    p.[client_id],
    c.[client_account_number],
    c.[client_name],
    p.[display_name],
    p.[context_label],
    p.[species],
    p.[breed],
    p.[sex],
    p.[birthdate],
    p.[active],
    p.[latest_visit],
    p.[flags],
    p.[preventive_flags]
from [dbo].[principal] as p
inner join [dbo].[client] as c
    on c.[client_id] = p.[client_id];