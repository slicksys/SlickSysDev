# management data domain map

## scope

This repository currently contains only the sql project file and build task wiring. The database schema itself is being analyzed from the active schema designer, and the first implementation slice is the lower-risk, read-first surface for scheduling, client/principal charting, and billing summary.

## canonical domains

### scheduling

- canonical tables: `ezt_appointments`, `reservations`, `clinic_schedule`, `ezt_resource_groups`
- supporting tables: `ezt_appt_*`, `ezt_block_*`, `appt_call_*`, `reservation_*`, `work_schedule_*`, `scheduled_*`
- purpose: resource and timeboard management

### client / principal

- canonical tables: `clients`, `patients`
- supporting tables: `client_defaults`, `patient_defaults`, `client_notes`, `client_locks`, `client_types`, `breeds`, `rabies_history`, `vaccine_patient_details`, `patient_rechecks`
- derived tables: `db_clients`, `db_patients`, `recent_patients`, `census_patient_log`, `census_patient_log_locations`
- purpose: billed account plus internal service-recipient record

### billing

- canonical tables: `trans_ar`, `trans_payments`, `trans_inventory`, `trans_products`, `trans_rx`, `trans_taxes`, `trans_codes`, `trans_interest`, `trans_nsf`
- supporting tables: `invoice_*`, `ar_*`, `user_invoice_*`
- derived tables: `invoice_listinfo`, `invoice_log`, `invoice_status`, `ar_build_history`, `ar_stmt_history`, `ar_common`, `ar_global_settings`
- purpose: ledgers, balances, invoice state, and payment posting

## terminology

- internal canonical term: `principal`
- veterinary ui label: `pet` or `patient`
- salon ui label: `guest` or `customer`
- billed account: `client`

## first api slice

### read models

- `schedule_board`
- `principal_summary`
- `billing_summary`

### write models

- `appointments`
- `reservations`
- `payments`

### preferred shape

- practice-scoped endpoints
- read-first projections
- no direct frontend dependency on `db_*` or summary tables
- writes return the updated read model when possible

## next steps

1. map each api contract to legacy source tables
2. define the first projection queries
3. identify which tables are command targets versus projection-only surfaces
4. add actual sql objects when the migration path is confirmed