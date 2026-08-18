# Azure Deployment Notes

## Target

- Subscription: `Visual Studio Professional`
- Tenant ID: `712013a5-270b-47ff-98e1-522b97690e81`
- Resource group: `rg-slicksys-prod`
- Container target: Azure Container Apps
- Recommended deployment region: `centralus`

The resource group metadata location is `eastus`, but the Azure SQL logical server `operion` is in `centralus`. Deploying Container Apps in `centralus` keeps the app close to the database and avoids unnecessary cross-region latency.

## Existing SQL

- SQL logical server: `operion`
- SQL fully qualified domain name: `operion.database.windows.net`
- SQL database: `free-sql-db-wr`
- Current tier: `GeneralPurpose`, `GP_S_Gen5_2`
- Current state observed from Cloud Shell: `Paused`

The paused state is expected for the free/serverless tier, but it can add wake-up delay on first database access after inactivity. This is acceptable for an initial deployment, but production traffic should use a paid tier or serverless settings that avoid user-visible database cold starts.

## Required Deployment Settings

Set these on the process that runs `aspire deploy` or in the GitHub Actions production environment:

```powershell
$env:Azure__SubscriptionId = "cda2aa60-c5cd-4791-8f3f-45255b2a3991"
$env:Azure__Location = "centralus"
$env:Azure__ResourceGroup = "rg-slicksys-prod"
```

## Required Secrets

Do not commit these values.

### ManagementData Connection String

Configuration key:

```text
ConnectionStrings__ManagementData
```

Shape:

```text
Server=tcp:operion.database.windows.net,1433;Initial Catalog=free-sql-db-wr;Persist Security Info=False;User ID=<sql-user>;Password=<sql-password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### ManagementData JWT Signing Key

Aspire parameter:

```text
managementdata-jwt-signing-key
```

Environment variable form:

```text
Parameters__managementdata_jwt_signing_key
```

Use a high-entropy secret value. Do not reuse the development placeholder currently present in appsettings.

## Preview Command

Run from the repository root:

```powershell
aspire deploy --apphost .\SlickSysDev.AppHost\SlickSysDev.AppHost.csproj --environment production --list-steps
```

## Deploy Command

Run only after Azure CLI authentication, SQL secrets, and deployment settings are configured:

```powershell
aspire deploy --apphost .\SlickSysDev.AppHost\SlickSysDev.AppHost.csproj --environment production
```

For non-interactive CI/CD, pass the required settings and secrets as environment variables and add `--non-interactive`.
