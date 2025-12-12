# Infrastructure as Code

This directory contains Infrastructure as Code (IaC) templates for deploying the Digital Marketing Agent to Azure.

## Structure

Infrastructure code will be organized based on the deployment strategy:

```
infra/
├── bicep/          # Azure Bicep templates
├── terraform/      # Terraform configurations (if used)
├── scripts/        # Deployment and utility scripts
└── docs/           # Infrastructure documentation
```

## Deployment Strategy

The infrastructure will be deployed using:
- **Azure Bicep** - Primary IaC language for Azure resources
- **Azure Developer CLI (azd)** - For orchestrated deployments
- **GitHub Actions** - For CI/CD pipelines

## Resources

Infrastructure will include:
- Azure App Service or Container Apps
- Azure SQL Database or Cosmos DB
- Azure Key Vault for secrets
- Azure Monitor and Application Insights
- Azure API Management (optional)
- Azure Storage for blobs/queues

## Getting Started

Infrastructure setup will begin during the deployment phase (Task: Azure Deployment).

**Placeholder:** IaC templates will be created by the Azure Agent.
