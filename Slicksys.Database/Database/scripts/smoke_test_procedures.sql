set nocount on;

declare @practice_id uniqueidentifier = '11111111-1111-1111-1111-111111111111';
declare @client_id uniqueidentifier = '22222222-2222-2222-2222-222222222222';
declare @principal_id uniqueidentifier = '33333333-3333-3333-3333-333333333333';
declare @resource_id uniqueidentifier = '44444444-4444-4444-4444-444444444444';
declare @appt_status_id uniqueidentifier = '55555555-5555-5555-5555-555555555555';
declare @res_status_id uniqueidentifier = '66666666-6666-6666-6666-666666666666';
declare @invoice_id uniqueidentifier = '77777777-7777-7777-7777-777777777777';
declare @now_dt datetime2(0) = cast(getdate() as datetime2(0));
declare @appt_end_dt datetime2(0) = dateadd(HOUR, 1, cast(getdate() as datetime2(0)));
declare @res_arrival_dt datetime2(0) = dateadd(DAY, 1, cast(getdate() as datetime2(0)));
declare @res_ending_dt datetime2(0) = dateadd(DAY, 2, cast(getdate() as datetime2(0)));

if not exists (select 1 from dbo.practice where practice_id = @practice_id)
begin
    insert into dbo.practice (practice_id, practice_name, is_active)
    values (@practice_id, N'smoke practice', 1);
end

if not exists (select 1 from dbo.client where client_id = @client_id)
begin
    insert into dbo.client (client_id, practice_id, client_account_number, client_name, billing_status, credit_limit, is_active)
    values (@client_id, @practice_id, N'SMOKE-1', N'smoke client', N'watch', 500.00, 1);
end

if not exists (select 1 from dbo.principal where principal_id = @principal_id)
begin
    insert into dbo.principal (principal_id, practice_id, client_id, display_name, context_label, species, breed, sex, active)
    values (@principal_id, @practice_id, @client_id, N'smoke principal', N'pet', N'canine', N'mix', N'M', 1);
end

if not exists (select 1 from dbo.resource where resource_id = @resource_id)
begin
    insert into dbo.resource (resource_id, practice_id, resource_name, resource_type, is_active)
    values (@resource_id, @practice_id, N'smoke room', N'room', 1);
end

if not exists (select 1 from dbo.appointment_status where status_id = @appt_status_id)
begin
    insert into dbo.appointment_status (status_id, practice_id, status_name, sort_order, is_active, color_code)
    values (@appt_status_id, @practice_id, N'scheduled', 1, 1, N'#00aaff');
end

if not exists (select 1 from dbo.reservation_status where status_id = @res_status_id)
begin
    insert into dbo.reservation_status (status_id, practice_id, status_name, sort_order, is_active, color_code)
    values (@res_status_id, @practice_id, N'booked', 1, 1, N'#00cc88');
end

if not exists (select 1 from dbo.invoice where invoice_id = @invoice_id)
begin
    insert into dbo.invoice (invoice_id, practice_id, client_id, invoice_number, invoice_date, due_date, status_name, total_amount, balance_amount, is_open)
    values (@invoice_id, @practice_id, @client_id, N'SMOKE-INV-1', cast(getdate() as date), cast(dateadd(day, 7, getdate()) as date), N'open', 100.00, 100.00, 1);
end

declare @appt table (
    appointment_id uniqueidentifier,
    practice_id uniqueidentifier,
    client_id uniqueidentifier,
    principal_id uniqueidentifier,
    resource_id uniqueidentifier,
    status_id uniqueidentifier,
    start_time datetime2(0),
    end_time datetime2(0),
    comments nvarchar(2000),
    group_id uniqueidentifier,
    recurrence_id uniqueidentifier,
    source nvarchar(50),
    is_deleted bit,
    created_at datetime2(0)
);

insert into @appt
exec dbo.usp_create_appointment
    @practice_id = @practice_id,
    @client_id = @client_id,
    @principal_id = @principal_id,
    @resource_id = @resource_id,
    @status_id = @appt_status_id,
    @start_time = @now_dt,
    @end_time = @appt_end_dt,
    @comments = N'smoke appointment',
    @source = N'smoke';

if (select count(1) from @appt) <> 1
    throw 51000, 'usp_create_appointment failed', 1;

declare @appt_id uniqueidentifier = (select top 1 appointment_id from @appt);

exec dbo.usp_update_appointment
    @practice_id = @practice_id,
    @appointment_id = @appt_id,
    @comments = N'smoke appointment updated';

declare @res table (
    reservation_id uniqueidentifier,
    practice_id uniqueidentifier,
    client_id uniqueidentifier,
    principal_id uniqueidentifier,
    arrival_date datetime2(0),
    ending_date datetime2(0),
    status_id uniqueidentifier,
    visual_status_id uniqueidentifier,
    hospitalized_flag bit,
    comments nvarchar(2000),
    source nvarchar(50),
    is_deleted bit,
    created_at datetime2(0)
);

insert into @res
exec dbo.usp_create_reservation
    @practice_id = @practice_id,
    @client_id = @client_id,
    @principal_id = @principal_id,
    @arrival_date = @res_arrival_dt,
    @ending_date = @res_ending_dt,
    @status_id = @res_status_id,
    @visual_status_id = @res_status_id,
    @hospitalized_flag = 0,
    @comments = N'smoke reservation',
    @source = N'smoke';

if (select count(1) from @res) <> 1
    throw 51001, 'usp_create_reservation failed', 1;

declare @res_id uniqueidentifier = (select top 1 reservation_id from @res);

exec dbo.usp_update_reservation
    @practice_id = @practice_id,
    @reservation_id = @res_id,
    @comments = N'smoke reservation updated';

declare @pay table (
    payment_id uniqueidentifier,
    practice_id uniqueidentifier,
    client_id uniqueidentifier,
    invoice_id uniqueidentifier,
    payment_amount decimal(18,2),
    payment_date datetime2(0),
    payment_method nvarchar(50),
    reference_number nvarchar(100),
    memo nvarchar(400),
    source nvarchar(50),
    posted_at datetime2(0)
);

insert into @pay
exec dbo.usp_post_payment
    @practice_id = @practice_id,
    @client_id = @client_id,
    @invoice_id = @invoice_id,
    @payment_amount = 20.00,
    @payment_date = @now_dt,
    @payment_method = N'card',
    @reference_number = N'SMOKE-REF',
    @memo = N'smoke payment',
    @source = N'smoke';

if (select count(1) from @pay) <> 1
    throw 51002, 'usp_post_payment failed', 1;

declare @schedule_count table (total_rows bigint);
insert into @schedule_count
exec dbo.usp_schedule_board_count
    @practice_id = @practice_id,
    @from_time = '2020-01-01T00:00:00',
    @to_time = '2035-01-01T00:00:00';

if (select top 1 total_rows from @schedule_count) < 1
    throw 51003, 'usp_schedule_board_count returned no rows', 1;

declare @principal_count table (total_rows bigint);
insert into @principal_count
exec dbo.usp_principal_summary_count
    @practice_id = @practice_id;

if (select top 1 total_rows from @principal_count) < 1
    throw 51004, 'usp_principal_summary_count returned no rows', 1;

declare @billing_count table (total_rows bigint);
insert into @billing_count
exec dbo.usp_billing_summary_count
    @practice_id = @practice_id;

if (select top 1 total_rows from @billing_count) < 1
    throw 51005, 'usp_billing_summary_count returned no rows', 1;

print 'smoke test passed';