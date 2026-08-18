# CI/CD and Azure Deployment Todo

## Task 1: Capture Current Build/Test Baseline

**Description:** Verify the current solution, AppHost, and test posture before deployment changes.

**Acceptance criteria:**
- [ ] `aspire ls` identifies `SlickSysDev.AppHost/SlickSysDev.AppHost.csproj`.
- [ ] Current build/test blockers are documented.
- [ ] Existing stale CI/CD and Azure scripts are identified.

**Verification:**
- [ ] Run `dotnet build .\SlickSysDev.slnx`.
- [ ] Run relevant tests once test dependencies are understood.

**Dependencies:** None

**Files likely touched:**
- `tasks/plan.md`
- `tasks/todo.md`

**Estimated scope:** Small

## Task 2: Decide Azure Topology and Naming

**Description:** Choose the production Azure target, region, resource group naming, app names, and endpoint exposure.

**Acceptance criteria:**
- [ ] Azure Container Apps target is confirmed or replaced by a conscious alternative.
- [ ] Subscription, region, resource group, and environment names are documented.
- [ ] Public/internal endpoint policy is documented for each AppHost resource.

**Verification:**
- [ ] No deployment occurs until the target subscription/resource group is confirmed.

**Dependencies:** Task 1

**Files likely touched:**
- `tasks/plan.md`
- Deployment documentation file to be added

**Estimated scope:** Small

## Task 3: Add Aspire Azure Container Apps Integration

**Description:** Add Azure Container Apps deployment support to the AppHost using Aspire-native deployment APIs.

**Acceptance criteria:**
- [ ] AppHost references the Azure Container Apps hosting integration.
- [ ] AppHost contains one Azure Container Apps environment resource.
- [ ] External endpoints are configured only for intended public apps.

**Verification:**
- [ ] `aspire deploy --apphost .\SlickSysDev.AppHost\SlickSysDev.AppHost.csproj --list-steps`
- [ ] `dotnet build .\SlickSysDev.slnx`

**Dependencies:** Task 2

**Files likely touched:**
- `SlickSysDev.AppHost/SlickSysDev.AppHost.csproj`
- `SlickSysDev.AppHost/AppHost.cs`

**Estimated scope:** Medium

## Task 4: Configure Production Runtime Readiness

**Description:** Ensure deployed apps expose appropriate health checks and runtime settings for Container Apps.

**Acceptance criteria:**
- [ ] Public apps have readiness/liveness behavior compatible with Azure Container Apps.
- [ ] Resource CPU/memory assumptions are documented.
- [ ] Logs and OpenTelemetry path are defined.

**Verification:**
- [ ] AppHost preview reports health endpoints for deployed resources.
- [ ] Azure deployment preview shows expected probes or health checks where supported.

**Dependencies:** Task 3

**Files likely touched:**
- `SlickSysDev.AppHost/AppHost.cs`
- `SlickSysDev.ServiceDefaults/Extensions.cs`
- App project configuration files as needed

**Estimated scope:** Medium

## Task 5: Configure Scale Settings to Avoid Cold Starts

**Description:** Configure minimum replicas and scale ceilings for public workloads so user visits do not wait for scale-from-zero startup.

**Acceptance criteria:**
- [ ] Public apps have `minReplicas` greater than zero.
- [ ] Production availability target determines whether public apps use one, two, or three minimum replicas.
- [ ] Non-public/background services have scale settings based on actual need.

**Verification:**
- [ ] Deployment preview shows expected scale configuration.
- [ ] Post-deployment `az containerapp` inspection confirms active replicas.

**Dependencies:** Task 3

**Files likely touched:**
- `SlickSysDev.AppHost/AppHost.cs`
- Deployment docs

**Estimated scope:** Medium

## Task 6: Move Production Secrets and Settings Out of Committed Config

**Description:** Replace committed production-sensitive settings with Aspire parameters, GitHub environment secrets, or managed Azure references.

**Acceptance criteria:**
- [ ] JWT signing key is not sourced from committed production config.
- [ ] ManagementData production connection string is provided via deployment settings.
- [ ] CI/CD lists required secrets without printing values.

**Verification:**
- [ ] Search confirms no production secrets are committed.
- [ ] `aspire deploy --list-steps` reports required parameters.

**Dependencies:** Task 3

**Files likely touched:**
- `SlickSysDev.AppHost/AppHost.cs`
- `SlickSysDev.Data.Api/appsettings*.json`
- GitHub workflow files

**Estimated scope:** Medium

## Task 7: Replace Stale Generated Workflow

**Description:** Remove or supersede the generated single-container workflow with an Aspire-native deployment workflow.

**Acceptance criteria:**
- [ ] Workflow uses `actions/checkout@v4`, `actions/setup-dotnet@v4`, Azure OIDC, and `aspire deploy`.
- [ ] Workflow pins the AppHost path.
- [ ] Workflow uses GitHub environment variables and secrets for Azure settings.

**Verification:**
- [ ] GitHub Actions YAML validates syntactically.
- [ ] Manual dispatch reaches deployment preview or deployment step as intended.

**Dependencies:** Tasks 3 and 6

**Files likely touched:**
- `.github/workflows/*.yml`

**Estimated scope:** Medium

## Task 8: Add Pull Request Validation Workflow

**Description:** Add a non-deploying workflow that validates build, tests, and deployment model shape for pull requests.

**Acceptance criteria:**
- [ ] PR workflow does not provision Azure resources.
- [ ] PR workflow runs restore/build/test.
- [ ] PR workflow runs Aspire deployment step preview.

**Verification:**
- [ ] Workflow passes on a branch before production workflow is enabled.

**Dependencies:** Tasks 1 and 3

**Files likely touched:**
- `.github/workflows/ci.yml`

**Estimated scope:** Medium

## Task 9: Add Protected Production Deployment Workflow

**Description:** Add production deployment automation gated by the GitHub `production` environment.

**Acceptance criteria:**
- [ ] Deployment runs only on approved branch/manual dispatch policy.
- [ ] Azure credentials use OIDC, not registry passwords.
- [ ] Deployment step uses `aspire deploy --environment production --non-interactive`.

**Verification:**
- [ ] GitHub environment protection is configured.
- [ ] First dry run reaches Azure authentication and deployment preview.

**Dependencies:** Tasks 6 and 7

**Files likely touched:**
- `.github/workflows/deploy-production.yml`

**Estimated scope:** Medium

## Task 10: Inventory Existing Azure Resources

**Description:** Identify existing Azure resources so cleanup is deliberate and reversible where possible.

**Acceptance criteria:**
- [ ] Active subscription and tenant are confirmed.
- [ ] Resource groups and resource types are listed.
- [ ] Candidate deletion scope is separated from resources to keep.

**Verification:**
- [ ] `az account show`
- [ ] `az group list`
- [ ] `az resource list` for candidate groups

**Dependencies:** Task 2

**Files likely touched:** None unless documenting results

**Estimated scope:** Small

## Task 11: Remove Approved Old Azure Resources

**Description:** Delete only the Azure resources/resource groups explicitly approved for cleanup.

**Acceptance criteria:**
- [ ] Deletion target is confirmed by subscription ID and resource group/resource names.
- [ ] Cleanup command output is captured.
- [ ] Post-cleanup inventory shows the approved resources removed.

**Verification:**
- [ ] `az group show` or `az resource show` confirms deletion state.

**Dependencies:** Task 10

**Files likely touched:** None

**Estimated scope:** Medium

## Task 12: Deploy and Verify Fresh Production

**Description:** Deploy the new Aspire-managed Azure environment and verify public endpoints.

**Acceptance criteria:**
- [ ] `aspire deploy` completes successfully.
- [ ] Public endpoints return healthy responses.
- [ ] Azure Container Apps revisions are healthy.
- [ ] Scale, logs, and alerts are verified.

**Verification:**
- [ ] `az containerapp list`
- [ ] Endpoint checks for public/admin/web apps
- [ ] Health checks and log inspection

**Dependencies:** Tasks 3 through 11

**Files likely touched:** Deployment docs if results are recorded

**Estimated scope:** Medium
