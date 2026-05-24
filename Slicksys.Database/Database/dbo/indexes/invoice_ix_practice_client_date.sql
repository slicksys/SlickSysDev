create index [ix_invoice_practice_client_date]
    on [dbo].[invoice] ([practice_id], [client_id], [invoice_date]);