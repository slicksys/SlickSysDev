# API to SQL Architecture Map

This document links HTTP endpoints to API controller actions, repository calls, and SQL stored procedures.

## runtime flow

1. Request enters ASP.NET Core pipeline.
2. JWT authentication validates token.
3. Policy handlers validate practice and role requirements.
4. Controller calls repository.
5. Repository executes stored procedure via Dapper.

## auth token flow

- Endpoint: `POST /api/v1/auth/token`
- Controller: `AuthController.CreateToken`
- Repository calls:
  - Credential lookup: direct `select` from `[dbo].[AspNetUsers]`
  - Practice authorization (optional): `dbo.usp_authorize_user_practice`
  - Practice role resolution (optional): `dbo.usp_user_practices_paged`
- Output: JWT from `JwtTokenService`

## practice access endpoints

- Endpoint: `GET /api/v1/practices/{practiceId}/users`
  - Policy: `PracticeMember`
  - Procedures:
    - `dbo.usp_practice_users_paged`
    - `dbo.usp_practice_users_count`

- Endpoint: `GET /api/v1/users/{userId}/practices`
  - Policy: `CurrentUserRoute`
  - Procedures:
    - `dbo.usp_user_practices_paged`
    - `dbo.usp_user_practices_count`

- Endpoint: `GET /api/v1/practices/{practiceId}/invitations`
  - Policy: `PracticeAdmin`
  - Procedures:
    - `dbo.usp_practice_invitations_paged`
    - `dbo.usp_practice_invitations_count`

- Endpoint: `GET /api/v1/practices/{practiceId}/authorization`
  - Policy: `SelfOrPracticeAdmin`
  - Procedure:
    - `dbo.usp_authorize_user_practice`

- Endpoint: `POST /api/v1/practices/{practiceId}/users/{userId}/roles`
  - Policy: `PracticeAdmin`
  - Procedure:
    - `dbo.usp_add_user_practice_role`

- Endpoint: `DELETE /api/v1/practices/{practiceId}/users/{userId}/roles/{roleName}`
  - Policy: `PracticeAdmin`
  - Procedure:
    - `dbo.usp_remove_user_practice_role`

- Endpoint: `POST /api/v1/practices/{practiceId}/invitations`
  - Policy: `PracticeAdmin`
  - Procedure:
    - `dbo.usp_create_user_practice_invitation`

- Endpoint: `POST /api/v1/invitations/{inviteToken}/accept`
  - Policy: authenticated user
  - Procedure:
    - `dbo.usp_accept_user_practice_invitation`

- Endpoint: `POST /api/v1/practices/{practiceId}/invitations/{inviteToken}/revoke`
  - Policy: `PracticeAdmin`
  - Procedure:
    - `dbo.usp_revoke_user_practice_invitation`

## scheduling endpoints

- Endpoint: `GET /api/v1/practices/{practiceId}/scheduling/schedule-board`
  - Policy: `PracticeMember`
  - Procedures:
    - `dbo.usp_schedule_board_paged`
    - `dbo.usp_schedule_board_count`

- Endpoint: `POST /api/v1/practices/{practiceId}/scheduling/appointments`
  - Policy: `PracticeUser`
  - Procedure:
    - `dbo.usp_create_appointment`

- Endpoint: `PATCH /api/v1/practices/{practiceId}/scheduling/appointments/{appointmentId}`
  - Policy: `PracticeUser`
  - Procedure:
    - `dbo.usp_update_appointment`

- Endpoint: `POST /api/v1/practices/{practiceId}/scheduling/reservations`
  - Policy: `PracticeUser`
  - Procedure:
    - `dbo.usp_create_reservation`

- Endpoint: `PATCH /api/v1/practices/{practiceId}/scheduling/reservations/{reservationId}`
  - Policy: `PracticeUser`
  - Procedure:
    - `dbo.usp_update_reservation`

## policy handlers and data contract

Policies are configured in `ServiceConfig` and implemented by handlers in `Features/Auth`.

- `PracticeAccessAuthorizationHandler` validates practice membership/role by calling `dbo.usp_authorize_user_practice` through `IAuthRepository`.
- `CurrentUserRouteAuthorizationHandler` enforces that `{userId}` route value matches the JWT subject.
- `SelfOrPracticeAdminAuthorizationHandler` allows self access or admin access to the target practice.

## integration test coverage

Integration tests in `tests/ManagementData.Api.IntegrationTests` validate:

- token issuance success and invalid password rejection
- unauthorized access without bearer token
- forbidden access for role-insufficient users
- not-found responses for missing invitation token and missing role assignment
- successful schedule-board retrieval with authorized token
