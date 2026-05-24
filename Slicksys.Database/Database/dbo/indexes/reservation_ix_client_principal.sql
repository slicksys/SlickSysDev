create index [ix_reservation_client_principal]
    on [dbo].[reservation] ([client_id], [principal_id]);