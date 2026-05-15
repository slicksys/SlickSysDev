# app query and command contract

## overview

This project now exposes a SQL contract through stored procedures under `dbo/stored_procedures` and reference scripts under `queries`.

Use procedures for runtime execution in application code and keep the ad hoc query scripts for exploration and troubleshooting.

## read procedures

- `dbo.usp_schedule_board_paged`
- `dbo.usp_schedule_board_count`
- `dbo.usp_principal_summary_paged`
- `dbo.usp_principal_summary_count`
- `dbo.usp_billing_summary_paged`
- `dbo.usp_billing_summary_count`
- `dbo.usp_practice_users_paged`
- `dbo.usp_practice_users_count`
- `dbo.usp_user_practices_paged`
- `dbo.usp_user_practices_count`
- `dbo.usp_practice_invitations_paged`
- `dbo.usp_practice_invitations_count`
- `dbo.usp_authorize_user_practice`

## write procedures

- `dbo.usp_create_appointment`
- `dbo.usp_update_appointment`
- `dbo.usp_create_reservation`
- `dbo.usp_update_reservation`
- `dbo.usp_post_payment`
- `dbo.usp_add_user_practice_role`
- `dbo.usp_remove_user_practice_role`
- `dbo.usp_create_user_practice_invitation`
- `dbo.usp_accept_user_practice_invitation`
- `dbo.usp_revoke_user_practice_invitation`

## identity and access conventions

- role assignment is enforced against `dbo.AspNetRoles`
- membership rows are practice-scoped in `dbo.user_practice_role`
- effective active membership means `is_active = 1` and `expires_at` is null or in the future
- pending invites are tracked in `dbo.user_practice_invitation` with status values: `pending`, `accepted`, `revoked`, `expired`
- authorization checks are available through `dbo.usp_authorize_user_practice` and `dbo.user_practice_access`

## parameter conventions

- required scope: `@practice_id`
- pagination: `@offset_rows`, `@fetch_rows`, `@sort_desc`
- optional filters should default to `null`

## domain conventions

- `client` is the billed account
- `principal` is the service recipient
- UI labels can vary by context (`pet`, `patient`, `guest`, `customer`) while the contract remains `principal`

## deployment notes

- lookup seeds are applied via `Script.PostDeployment.sql`
- seed source lives in `scripts/seed_lookup_data.sql`
