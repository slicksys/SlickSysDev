create index [ix_payment_client_date]
    on [dbo].[payment] ([client_id], [payment_date]);