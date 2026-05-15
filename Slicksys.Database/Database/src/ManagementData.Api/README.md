# ManagementData API

## overview

This project is the ASP.NET Core backend API for the SQL contract in the parent `ManagementData.sqlproj`.

The API is SQL-first: repositories call stored procedures in the database project, and policy-based authorization guards access by practice membership and role.

## current endpoints

### auth

- `POST /api/v1/auth/token`

### practice access

- `GET /api/v1/practices/{practiceId}/users`
- `GET /api/v1/users/{userId}/practices`
- `GET /api/v1/practices/{practiceId}/invitations`
- `GET /api/v1/practices/{practiceId}/authorization`
- `POST /api/v1/practices/{practiceId}/users/{userId}/roles`
- `DELETE /api/v1/practices/{practiceId}/users/{userId}/roles/{roleName}`
- `POST /api/v1/practices/{practiceId}/invitations`
- `POST /api/v1/invitations/{inviteToken}/accept`
- `POST /api/v1/practices/{practiceId}/invitations/{inviteToken}/revoke`

### scheduling

- `GET /api/v1/practices/{practiceId}/scheduling/schedule-board`
- `POST /api/v1/practices/{practiceId}/scheduling/appointments`
- `PATCH /api/v1/practices/{practiceId}/scheduling/appointments/{appointmentId}`
- `POST /api/v1/practices/{practiceId}/scheduling/reservations`
- `PATCH /api/v1/practices/{practiceId}/scheduling/reservations/{reservationId}`

### platform

- `GET /health`

## auth and authorization

Authentication uses JWT Bearer tokens.

- Token issuance validates username/email + password against `AspNetUsers.PasswordHash`.
- Optional practice scoping during token creation validates the caller through `dbo.usp_authorize_user_practice`.

Authorization is policy based, with centralized handlers.

- `PracticeMember`
- `PracticeUser`
- `PracticeAdmin`
- `CurrentUserRoute`
- `SelfOrPracticeAdmin`

## configuration

Set `ConnectionStrings:ManagementData` to the target SQL Server database.

The development default points to the local e2e database used by the SQL project scripts.

## local run

```powershell
dotnet run --project .\src\ManagementData.Api\ManagementData.Api.csproj
```

## testing

Integration tests live in `tests/ManagementData.Api.IntegrationTests`.

Current coverage includes:

- token success and invalid-credential failure paths
- unauthorized/forbidden authorization paths
- not-found paths for invitation revoke and role removal
- scheduling read path with authorized token

Run tests:

```powershell
dotnet test .\tests\ManagementData.Api.IntegrationTests\ManagementData.Api.IntegrationTests.csproj
```

## architecture map

See `docs/integration/api-sql-architecture-map.md` for endpoint to stored procedure mapping.