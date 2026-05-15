create procedure [dbo].[usp_post_payment]
    @practice_id uniqueidentifier,
    @client_id uniqueidentifier,
    @invoice_id uniqueidentifier = null,
    @payment_amount decimal(18, 2),
    @payment_date datetime2(0),
    @payment_method nvarchar(50),
    @reference_number nvarchar(100) = null,
    @memo nvarchar(400) = null,
    @source nvarchar(50) = null
as
begin
    set nocount on;

    insert into [dbo].[payment] (
        [practice_id],
        [client_id],
        [invoice_id],
        [payment_amount],
        [payment_date],
        [payment_method],
        [reference_number],
        [memo],
        [source]
    )
    output
        inserted.[payment_id],
        inserted.[practice_id],
        inserted.[client_id],
        inserted.[invoice_id],
        inserted.[payment_amount],
        inserted.[payment_date],
        inserted.[payment_method],
        inserted.[reference_number],
        inserted.[memo],
        inserted.[source],
        inserted.[posted_at]
    values (
        @practice_id,
        @client_id,
        @invoice_id,
        @payment_amount,
        @payment_date,
        @payment_method,
        @reference_number,
        @memo,
        coalesce(@source, N'new')
    );
end