# query starters

These query scripts are the first read-side entry points for the new lowercase model.

## included queries

- [schedule_board.sql](schedule_board.sql)
- [principal_summary.sql](principal_summary.sql)
- [billing_summary.sql](billing_summary.sql)
- [schedule_board_paged.sql](schedule_board_paged.sql)
- [schedule_board_count.sql](schedule_board_count.sql)
- [principal_summary_paged.sql](principal_summary_paged.sql)
- [principal_summary_count.sql](principal_summary_count.sql)
- [billing_summary_paged.sql](billing_summary_paged.sql)
- [billing_summary_count.sql](billing_summary_count.sql)
- [create_appointment.sql](create_appointment.sql)
- [update_appointment.sql](update_appointment.sql)
- [create_reservation.sql](create_reservation.sql)
- [update_reservation.sql](update_reservation.sql)
- [post_payment.sql](post_payment.sql)
- [clinical_soap_timeline.sql](clinical_soap_timeline.sql)
- [clinical_soap_search.sql](clinical_soap_search.sql)
- [inventory_product_catalog.sql](inventory_product_catalog.sql)
- [inventory_product_detail.sql](inventory_product_detail.sql)
- [clinical_soap_timeline_paged.sql](clinical_soap_timeline_paged.sql)
- [clinical_soap_timeline_count.sql](clinical_soap_timeline_count.sql)
- [clinical_notes_recent_paged.sql](clinical_notes_recent_paged.sql)
- [clinical_diagnosis_recent_paged.sql](clinical_diagnosis_recent_paged.sql)
- [clinical_forms_catalog_paged.sql](clinical_forms_catalog_paged.sql)
- [clinical_imaging_mr_images_paged.sql](clinical_imaging_mr_images_paged.sql)
- [clinical_lab_results_paged.sql](clinical_lab_results_paged.sql)
- [inventory_product_catalog_paged.sql](inventory_product_catalog_paged.sql)
- [inventory_product_catalog_count.sql](inventory_product_catalog_count.sql)
- [inventory_product_detail_paged.sql](inventory_product_detail_paged.sql)
- [add_user_practice_role.sql](add_user_practice_role.sql)
- [remove_user_practice_role.sql](remove_user_practice_role.sql)
- [practice_users_paged.sql](practice_users_paged.sql)
- [practice_users_count.sql](practice_users_count.sql)
- [user_practices_paged.sql](user_practices_paged.sql)
- [user_practices_count.sql](user_practices_count.sql)
- [create_user_practice_invitation.sql](create_user_practice_invitation.sql)
- [accept_user_practice_invitation.sql](accept_user_practice_invitation.sql)
- [revoke_user_practice_invitation.sql](revoke_user_practice_invitation.sql)
- [practice_invitations_paged.sql](practice_invitations_paged.sql)
- [practice_invitations_count.sql](practice_invitations_count.sql)
- [authorize_user_practice.sql](authorize_user_practice.sql)

## query groups

### scheduling

- `schedule_board.sql`
- `schedule_board_paged.sql`
- `schedule_board_count.sql`
- `create_appointment.sql`
- `update_appointment.sql`
- `create_reservation.sql`
- `update_reservation.sql`
- `post_payment.sql`

### clinical

- `clinical_soap_timeline.sql`
- `clinical_soap_search.sql`
- `clinical_notes_recent.sql`
- `clinical_notes_by_principal.sql`
- `clinical_diagnosis_recent.sql`
- `clinical_diagnosis_by_principal.sql`
- `clinical_principal_activity_summary.sql`
- `clinical_forms_catalog.sql`
- `clinical_forms_categories.sql`
- `clinical_imaging_mr_images.sql`
- `clinical_imaging_dicom_worklist.sql`
- `clinical_lab_results.sql`
- `clinical_soap_timeline_paged.sql`
- `clinical_soap_timeline_count.sql`
- `clinical_notes_recent_paged.sql`
- `clinical_diagnosis_recent_paged.sql`
- `clinical_forms_catalog_paged.sql`
- `clinical_imaging_mr_images_paged.sql`
- `clinical_lab_results_paged.sql`

### inventory

- `inventory_product_catalog.sql`
- `inventory_product_detail.sql`
- `inventory_product_catalog_paged.sql`
- `inventory_product_catalog_count.sql`
- `inventory_product_detail_paged.sql`

### account and chart

- `principal_summary.sql`
- `principal_summary_paged.sql`
- `principal_summary_count.sql`
- `billing_summary.sql`
- `billing_summary_paged.sql`
- `billing_summary_count.sql`

### identity and access

- `add_user_practice_role.sql`
- `remove_user_practice_role.sql`
- `practice_users_paged.sql`
- `practice_users_count.sql`
- `user_practices_paged.sql`
- `user_practices_count.sql`
- `create_user_practice_invitation.sql`
- `accept_user_practice_invitation.sql`
- `revoke_user_practice_invitation.sql`
- `practice_invitations_paged.sql`
- `practice_invitations_count.sql`
- `authorize_user_practice.sql`

## parameter conventions

- `@practice_id` is required for all queries
- `@from_time` and `@to_time` are used by the schedule board query
- `@client_id` and `@principal_id` are optional filters where noted
- paged query variants use:
	- `@offset_rows` (int)
	- `@fetch_rows` (int)
	- `@sort_desc` (bit)

## next step

Use these scripts as the baseline for data exploration and simple command execution before adding stored procedures or service wrappers.