# SlickSys Development

## Strategic Assessment & Modernization Planning
Success in digital transformation begins with a comprehensive understanding of your business objectives, technology landscape, and transformation goals. This is often the most challenging phase of any modernization initiative. Our expert assessment process separates the wheat from the chaff and creates the foundation for risk-managed, cost-effective modernization.

- Business Value Analysis & ROI Mapping  
- Technology Stack Evaluation  
- Risk Assessment & Mitigation Strategy  
- Compliance & Security Review  

## Software Development
Whether it's AWS or Azure, C# or C++, SQL or NoSQL, our approach to development combines subject matter expertise, enterprise-grade architecture, and cutting-edge technologies to deliver scalable, maintainable solutions that drive business value. We design robust systems that can easily grow with your business while reducing technical debt and maintenance costs.

- Cloud-Native Applications  
- Microservices Architecture  
- API Development & Integration  
- DevSecOps Implementation  

**Technologies:** Vue.js, Node.js, Java, React

## Data Engineering & Analytics
Businesses often struggle with fragmented data across multiple systems, platforms, and databases. We transform this complex landscape into a unified, actionable source of truth. Our platform-agnostic approach ensures all your data assets—regardless of source or format—contribute to your business intelligence strategy.

- Data Platform Modernization & Integration  
- ELT/ETL Pipelines & Orchestration  
- Data Modeling, Warehousing & Lakehouse Patterns  
- Analytics Enablement & Self-Service BI  
- Data Quality, Governance & Observability  

**Technologies:** Vue.js, Node.js, Java, React

## DevOps & Integration
Regardless of platform, Agile development strategies depend on modern DevOps as an essential component of modern software development. Automated pipelines eliminate manual errors, reduce deployment risks, and transform release management from a source of stress into a competitive advantage—whether the solution is a large monolithic application or involves complex containerized distribution. Through automated and continuous integration and deployment (CI/CD), we help organizations deliver value faster and more reliably.

- CI/CD Pipeline Design & Automation  
- Infrastructure as Code (IaC)  
- Containerization & Deployment Strategy  
- Release Engineering & Environment Promotion  
- Monitoring, Alerting & Reliability Practices  

## Azure Container Apps Deployment
This repo now includes a production-oriented Azure Container Apps deployment flow:

- [Dockerfile](Dockerfile) builds the application image for runtime use.
- [.github/workflows/deploy-aca.yml](.github/workflows/deploy-aca.yml) deploys on every push to `main`.
- [scripts/azure/bootstrap-aca.sh](scripts/azure/bootstrap-aca.sh) provisions the Azure prerequisites and prints the GitHub secrets and variables to add.
- [scripts/azure/deploy-aca.sh](scripts/azure/deploy-aca.sh) builds in Azure Container Registry and creates or updates the Container App.

### One-time Azure bootstrap
Run this after signing in with `az login`:

```bash
export AZURE_LOCATION=eastus
export AZURE_RESOURCE_GROUP=rg-slicksys-prod
export AZURE_ACR_NAME=slicksysprodacr
export AZURE_CONTAINERAPPS_ENVIRONMENT=cae-slicksys-prod
export AZURE_CONTAINER_APP_NAME=slicksys-web
export GITHUB_REPOSITORY=slicksys/SlickSysCorp

./scripts/azure/bootstrap-aca.sh
```

The script creates:

- a resource group
- an Azure Container Registry with ARM authentication enabled
- an Azure Container Apps environment
- a runtime managed identity for ACR pulls
- a GitHub deployment managed identity with OIDC federation for the `production` environment

### GitHub configuration
Create a GitHub environment named `production` and add the secrets and variables printed by the bootstrap script.

The deployment workflow uses GitHub OIDC with the Azure user-assigned managed identity created by `./scripts/azure/bootstrap-aca.sh`. The `AZURE_CLIENT_ID` secret should be the client ID of that managed identity, not a client secret and not a separate app registration unless you intentionally switch auth models.

If deployment fails during `azure/login` with `AADSTS70025`, the federated credential is missing or does not match the workflow subject. Check the identity named by `AZURE_GITHUB_IDENTITY_NAME` under Azure Portal -> Managed Identities -> Federated credentials and confirm it trusts:

- issuer: `https://token.actions.githubusercontent.com`
- subject: `repo:slicksys/SlickSysCorp:environment:production`
- audience: `api://AzureADTokenExchange`

The bootstrap script creates this federated credential automatically. If you add it manually in the portal, use the GitHub Actions template for Azure resources and target the `slicksys` / `SlickSysCorp` repository and the `production` environment.

Recommended protection:

- restrict the `production` environment to the `main` branch

### Deployment behavior
When `main` is updated, the workflow:

- signs in to Azure with OIDC via `azure/login`
- runs `az acr build` using the repo `Dockerfile`
- creates the Container App if it does not exist yet
- updates the Container App image when it already exists
