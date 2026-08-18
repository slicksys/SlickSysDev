# Implementation Plan: CI/CD and Azure Deployment Foundation

## Overview

Prepare SlickSysDev for a clean Azure deployment from the Aspire AppHost, with containerized apps, protected CI/CD, and production settings that avoid user-facing cold starts. The recommended target is Azure Container Apps because the solution is already an Aspire distributed app with multiple web/API resources and Redis.

## Architecture Decisions

- Use the Aspire AppHost as the deployment source of truth instead of the existing single-app Azure script/workflow under `SlickSysDev.Public`.
- Deploy to Azure Container Apps with at least one always-ready replica for public apps; use higher minimum replica counts for production availability targets.
- Use managed Azure services for production dependencies where practical, starting with managed cache/database decisions rather than deploying development-only local resources.
- Use GitHub Actions with Azure OIDC and a protected `production` environment for deployments.
- Keep destructive cleanup of existing Azure resources separate from the new deployment workflow until the target subscription/resource groups are confirmed.

## Task List

### Phase 1: Deployment Baseline

- [ ] Task 1: Capture current build/test baseline and app graph
- [ ] Task 2: Decide target Azure topology and naming
- [ ] Task 3: Add Aspire Azure Container Apps deployment integration

### Checkpoint: Deployment Model

- [ ] `aspire ls` identifies one buildable AppHost
- [ ] `aspire deploy --list-steps` shows the Azure deployment pipeline
- [ ] Public/internal endpoints are documented before deployment

### Phase 2: Container and Runtime Hardening

- [ ] Task 4: Configure production health checks, probes, and resource sizing
- [ ] Task 5: Configure scale limits to avoid cold starts for public apps
- [ ] Task 6: Move production secrets/settings out of committed appsettings

### Checkpoint: Runtime Readiness

- [ ] `dotnet build` succeeds
- [ ] AppHost deployment preview succeeds
- [ ] Secrets and parameters are inventoried without exposing values

### Phase 3: CI/CD

- [ ] Task 7: Replace stale generated workflow with Aspire-native GitHub Actions
- [ ] Task 8: Add validation workflow for pull requests
- [ ] Task 9: Add protected production deployment workflow

### Checkpoint: Pipeline Readiness

- [ ] PR workflow restores, builds, and validates deployment steps
- [ ] Production workflow uses Azure OIDC and GitHub environment protection
- [ ] Deployment workflow does not require registry passwords or committed secrets

### Phase 4: Azure Cleanup and First Deployment

- [ ] Task 10: Inventory existing Azure resource groups and resources
- [ ] Task 11: Confirm deletion scope and remove old Azure resources
- [ ] Task 12: Deploy fresh production environment and verify endpoints

### Checkpoint: Production Ready

- [ ] Old approved resources are removed
- [ ] New resources are tagged and grouped consistently
- [ ] Public endpoints pass health checks
- [ ] Scale settings, logs, and alerts are verified

## Risks and Mitigations

| Risk | Impact | Mitigation |
| --- | --- | --- |
| Existing Azure resources are shared with unrelated workloads | High | Inventory first; delete only explicitly approved resource groups/resources |
| Cold starts affect public apps | High | Configure minimum replicas greater than zero; consider three replicas for high availability |
| AppHost uses development dependencies in production | High | Replace local/dev connection strings with deployment parameters and managed Azure resources |
| Duplicate or stale projects confuse pipelines | Medium | Pin CI/CD to `SlickSysDev.slnx` and `SlickSysDev.AppHost/SlickSysDev.AppHost.csproj` |
| Azure/Aspire deployment APIs drift | Medium | Verify current Aspire docs before editing AppHost deployment APIs |

## Open Questions

- Which Azure subscription should host the new environment?
- Preferred Azure region, for example `eastus`, `eastus2`, or another region near the user base?
- What production domain names should map to `public`, `admin`, and any customer-facing app?
- Should `admin` be public with authentication, restricted by network access, or internal-only behind a private endpoint/VPN?
- What database should production use for `ManagementData`: Azure SQL Database, SQL Managed Instance, or an existing SQL Server migration path?
