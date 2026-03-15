#!/usr/bin/env bash

set -euo pipefail

require_env() {
  local name="$1"
  if [[ -z "${!name:-}" ]]; then
    echo "Missing required environment variable: ${name}" >&2
    exit 1
  fi
}

require_env AZURE_RESOURCE_GROUP
require_env AZURE_CONTAINERAPPS_ENVIRONMENT
require_env AZURE_CONTAINER_APP_NAME
require_env AZURE_ACR_NAME
require_env AZURE_RUNTIME_IDENTITY_NAME

AZURE_IMAGE_NAME="${AZURE_IMAGE_NAME:-${AZURE_CONTAINER_APP_NAME}}"
IMAGE_TAG="${IMAGE_TAG:-${GITHUB_SHA:-manual}}"

az config set extension.use_dynamic_install=yes_without_prompt >/dev/null
az extension add --name containerapp --upgrade >/dev/null

acr_login_server="$(az acr show --name "${AZURE_ACR_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query loginServer --output tsv)"
runtime_identity_id="$(az identity show --name "${AZURE_RUNTIME_IDENTITY_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query id --output tsv)"
image_ref="${acr_login_server}/${AZURE_IMAGE_NAME}:${IMAGE_TAG}"

az acr build \
  --registry "${AZURE_ACR_NAME}" \
  --image "${AZURE_IMAGE_NAME}:${IMAGE_TAG}" \
  --file Dockerfile \
  .

if az containerapp show \
  --name "${AZURE_CONTAINER_APP_NAME}" \
  --resource-group "${AZURE_RESOURCE_GROUP}" \
  >/dev/null 2>&1; then
  az containerapp update \
    --name "${AZURE_CONTAINER_APP_NAME}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --image "${image_ref}" \
    >/dev/null
else
  az containerapp create \
    --name "${AZURE_CONTAINER_APP_NAME}" \
    --resource-group "${AZURE_RESOURCE_GROUP}" \
    --environment "${AZURE_CONTAINERAPPS_ENVIRONMENT}" \
    --image "${image_ref}" \
    --target-port 8080 \
    --ingress external \
    --registry-server "${acr_login_server}" \
    --user-assigned "${runtime_identity_id}" \
    --registry-identity "${runtime_identity_id}" \
    --cpu 0.5 \
    --memory 1.0Gi \
    --min-replicas 1 \
    --max-replicas 3 \
    --env-vars ASPNETCORE_ENVIRONMENT=Production \
    >/dev/null
fi

fqdn="$(az containerapp show --name "${AZURE_CONTAINER_APP_NAME}" --resource-group "${AZURE_RESOURCE_GROUP}" --query properties.configuration.ingress.fqdn --output tsv)"

if [[ -n "${GITHUB_OUTPUT:-}" ]]; then
  echo "url=https://${fqdn}" >> "${GITHUB_OUTPUT}"
  echo "image=${image_ref}" >> "${GITHUB_OUTPUT}"
fi

echo "Deployed ${image_ref}"
echo "Application URL: https://${fqdn}"
