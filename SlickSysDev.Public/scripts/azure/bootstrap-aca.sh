#!/usr/bin/env bash

set -euo pipefail

require_env() {
  local name="$1"
  if [[ -z "${!name:-}" ]]; then
    echo "Missing required environment variable: ${name}" >&2
    exit 1
  fi
}

require_env AZURE_LOCATION
require_env AZURE_RESOURCE_GROUP
require_env AZURE_ACR_NAME
require_env AZURE_CONTAINERAPPS_ENVIRONMENT
require_env AZURE_CONTAINER_APP_NAME
require_env GITHUB_REPOSITORY

GITHUB_ENVIRONMENT="${GITHUB_ENVIRONMENT:-production}"
AZURE_ACR_SKU="${AZURE_ACR_SKU:-Basic}"
AZURE_RUNTIME_IDENTITY_NAME="${AZURE_RUNTIME_IDENTITY_NAME:-${AZURE_CONTAINER_APP_NAME}-runtime}"
AZURE_GITHUB_IDENTITY_NAME="${AZURE_GITHUB_IDENTITY_NAME:-${AZURE_CONTAINER_APP_NAME}-github}"

az config set extension.use_dynamic_install=yes_without_prompt >/dev/null
az extension add --name containerapp --upgrade >/dev/null

az provider register --namespace Microsoft.App --wait >/dev/null
az provider register --namespace Microsoft.OperationalInsights --wait >/dev/null
az provider register --namespace Microsoft.ContainerRegistry --wait >/dev/null
az provider register --namespace Microsoft.ManagedIdentity --wait >/dev/null

az group create \
  --name "${AZURE_RESOURCE_GROUP}" \
  --location "${AZURE_LOCATION}" \
  >/dev/null

if ! az acr show \
  --name "${AZURE_ACR_NAME}" \
  --resource-group "${AZURE_RESOURCE_GROUP}" \
  >/dev/null 2>&1; then
  az acr create \
    --name "${AZURE_ACR_NAME}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --location "${AZURE_LOCATION}" \
    --sku "${AZURE_ACR_SKU}" \
    --admin-enabled false \
    >/dev/null
fi

az acr config authentication-as-arm update \
  --registry "${AZURE_ACR_NAME}" \
  --status enabled \
  >/dev/null

if ! az containerapp env show \
  --name "${AZURE_CONTAINERAPPS_ENVIRONMENT}" \
  --resource-group "${AZURE_RESOURCE_GROUP}" \
  >/dev/null 2>&1; then
  az containerapp env create \
    --name "${AZURE_CONTAINERAPPS_ENVIRONMENT}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --location "${AZURE_LOCATION}" \
    >/dev/null
fi

if ! az identity show \
  --name "${AZURE_RUNTIME_IDENTITY_NAME}" \
  --resource-group "${AZURE_RESOURCE_GROUP}" \
  >/dev/null 2>&1; then
  az identity create \
    --name "${AZURE_RUNTIME_IDENTITY_NAME}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --location "${AZURE_LOCATION}" \
    >/dev/null
fi

if ! az identity show \
  --name "${AZURE_GITHUB_IDENTITY_NAME}" \
  --resource-group "${AZURE_RESOURCE_GROUP}" \
  >/dev/null 2>&1; then
  az identity create \
    --name "${AZURE_GITHUB_IDENTITY_NAME}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --location "${AZURE_LOCATION}" \
    >/dev/null
fi

acr_id="$(az acr show --name "${AZURE_ACR_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query id --output tsv)"
runtime_principal_id="$(az identity show --name "${AZURE_RUNTIME_IDENTITY_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query principalId --output tsv)"
github_principal_id="$(az identity show --name "${AZURE_GITHUB_IDENTITY_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query principalId --output tsv)"
github_client_id="$(az identity show --name "${AZURE_GITHUB_IDENTITY_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query clientId --output tsv)"
subscription_id="$(az account show --query id --output tsv)"
tenant_id="$(az account show --query tenantId --output tsv)"

az role assignment create \
  --assignee-object-id "${runtime_principal_id}" \
  --assignee-principal-type ServicePrincipal \
  --role AcrPull \
  --scope "${acr_id}" \
  >/dev/null || true

az role assignment create \
  --assignee-object-id "${github_principal_id}" \
  --assignee-principal-type ServicePrincipal \
  --role AcrPush \
  --scope "${acr_id}" \
  >/dev/null || true

az role assignment create \
  --assignee-object-id "${github_principal_id}" \
  --assignee-principal-type ServicePrincipal \
  --role Contributor \
  --scope "/subscriptions/${subscription_id}/resourceGroups/${AZURE_RESOURCE_GROUP}" \
  >/dev/null || true

if ! az identity federated-credential show \
  --name github-${GITHUB_ENVIRONMENT} \
  --identity-name "${AZURE_GITHUB_IDENTITY_NAME}" \
  --resource-group "${AZURE_RESOURCE_GROUP}" \
  >/dev/null 2>&1; then
  az identity federated-credential create \
    --name github-${GITHUB_ENVIRONMENT} \
    --identity-name "${AZURE_GITHUB_IDENTITY_NAME}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --issuer "https://token.actions.githubusercontent.com" \
    --subject "repo:${GITHUB_REPOSITORY}:environment:${GITHUB_ENVIRONMENT}" \
    --audiences "api://AzureADTokenExchange" \
    >/dev/null
fi

cat <<EOF
Bootstrap complete.

Create a GitHub environment named ${GITHUB_ENVIRONMENT} and add these secrets:
  AZURE_CLIENT_ID=${github_client_id}
  AZURE_TENANT_ID=${tenant_id}
  AZURE_SUBSCRIPTION_ID=${subscription_id}

Add these GitHub environment variables:
  AZURE_RESOURCE_GROUP=${AZURE_RESOURCE_GROUP}
  AZURE_CONTAINERAPPS_ENVIRONMENT=${AZURE_CONTAINERAPPS_ENVIRONMENT}
  AZURE_CONTAINER_APP_NAME=${AZURE_CONTAINER_APP_NAME}
  AZURE_ACR_NAME=${AZURE_ACR_NAME}
  AZURE_RUNTIME_IDENTITY_NAME=${AZURE_RUNTIME_IDENTITY_NAME}
  AZURE_IMAGE_NAME=${AZURE_CONTAINER_APP_NAME}

Recommended GitHub environment protection:
  - Environment name: ${GITHUB_ENVIRONMENT}
  - Restrict deployments to the main branch

OIDC federation details for troubleshooting:
  - Managed identity name: ${AZURE_GITHUB_IDENTITY_NAME}
  - Issuer: https://token.actions.githubusercontent.com
  - Subject: repo:${GITHUB_REPOSITORY}:environment:${GITHUB_ENVIRONMENT}
  - Audience: api://AzureADTokenExchange
EOF
