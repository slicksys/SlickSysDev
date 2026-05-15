merge [dbo].[principal_context_label] as target
using (
    values
        (N'pet', N'Pet', cast(1 as bit)),
        (N'patient', N'Patient', cast(1 as bit)),
        (N'guest', N'Guest', cast(1 as bit)),
        (N'customer', N'Customer', cast(1 as bit)),
        (N'principal', N'Principal', cast(1 as bit))
) as source ([context_label], [display_name], [is_active])
on target.[context_label] = source.[context_label]
when matched then
    update set
        target.[display_name] = source.[display_name],
        target.[is_active] = source.[is_active]
when not matched by target then
    insert ([context_label], [display_name], [is_active])
    values (source.[context_label], source.[display_name], source.[is_active]);

merge [dbo].[resource_type] as target
using (
    values
        (N'provider', N'Provider', cast(1 as bit)),
        (N'room', N'Room', cast(1 as bit)),
        (N'table', N'Table', cast(1 as bit)),
        (N'suite', N'Suite', cast(1 as bit)),
        (N'other', N'Other', cast(1 as bit))
) as source ([resource_type], [display_name], [is_active])
on target.[resource_type] = source.[resource_type]
when matched then
    update set
        target.[display_name] = source.[display_name],
        target.[is_active] = source.[is_active]
when not matched by target then
    insert ([resource_type], [display_name], [is_active])
    values (source.[resource_type], source.[display_name], source.[is_active]);

merge [dbo].[payment_method] as target
using (
    values
        (N'cash', N'Cash', cast(1 as bit)),
        (N'card', N'Card', cast(1 as bit)),
        (N'check', N'Check', cast(1 as bit)),
        (N'ach', N'ACH', cast(1 as bit)),
        (N'other', N'Other', cast(1 as bit))
) as source ([payment_method], [display_name], [is_active])
on target.[payment_method] = source.[payment_method]
when matched then
    update set
        target.[display_name] = source.[display_name],
        target.[is_active] = source.[is_active]
when not matched by target then
    insert ([payment_method], [display_name], [is_active])
    values (source.[payment_method], source.[display_name], source.[is_active]);

merge [dbo].[invoice_status_lookup] as target
using (
    values
        (N'open', N'Open', cast(1 as bit)),
        (N'closed', N'Closed', cast(1 as bit)),
        (N'void', N'Void', cast(1 as bit)),
        (N'past_due', N'Past Due', cast(1 as bit))
) as source ([invoice_status], [display_name], [is_active])
on target.[invoice_status] = source.[invoice_status]
when matched then
    update set
        target.[display_name] = source.[display_name],
        target.[is_active] = source.[is_active]
when not matched by target then
    insert ([invoice_status], [display_name], [is_active])
    values (source.[invoice_status], source.[display_name], source.[is_active]);

merge [dbo].[AspNetRoles] as target
using (
    values
        (N'role_practice_admin', N'practice_admin', N'PRACTICE_ADMIN', cast(null as nvarchar(max))),
        (N'role_practice_manager', N'practice_manager', N'PRACTICE_MANAGER', cast(null as nvarchar(max))),
        (N'role_practice_user', N'practice_user', N'PRACTICE_USER', cast(null as nvarchar(max))),
        (N'role_billing_user', N'billing_user', N'BILLING_USER', cast(null as nvarchar(max))),
        (N'role_read_only', N'read_only', N'READ_ONLY', cast(null as nvarchar(max)))
) as source ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
on target.[Id] = source.[Id]
when matched then
    update set
        target.[Name] = source.[Name],
        target.[NormalizedName] = source.[NormalizedName],
        target.[ConcurrencyStamp] = source.[ConcurrencyStamp]
when not matched by target then
    insert ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    values (source.[Id], source.[Name], source.[NormalizedName], source.[ConcurrencyStamp]);