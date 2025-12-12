# Digital Marketing Agent 🚀

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.md)
[![PRD](https://img.shields.io/badge/docs-PRD-green)](specs/prd.md)
[![Project Status](https://img.shields.io/badge/status-in%20development-yellow)]()

> Intelligent automation system for lead generation and engagement in legal services

## 📋 Overview

The **Digital Marketing Agent** is an intelligent automation system designed to streamline lead generation and initial engagement for legal services businesses. It manages multi-channel lead capture, qualification, and routing while maintaining data quality for downstream sales and legal teams.

### Key Features

- 🎯 **Multi-Channel Lead Generation** - Digital Ads, PR, Business Development, Organic Marketing
- 🔍 **Lead Qualification & Routing** - Intelligent lead scoring and automated routing to Sales Agent
- 📊 **Analytics & Reporting** - Real-time performance metrics and ROI tracking
- 🔗 **Sales Agent Integration** - Seamless handoff with complete contextual information
- ✅ **Data Quality Assurance** - Validation and enrichment before case creation
- 🌐 **Multiple Lead Sources** - Developer Leads, Referral Partners, Website Forms

### Business Goals

- **40% increase** in lead volume within 6 months
- **30% reduction** in unqualified leads reaching Sales team
- **50% faster** lead-to-case conversion time
- **60% reduction** in manual lead management effort

## 🏗️ Project Structure

```
marketing-agent/
├── src/
│   ├── backend/        # Backend application code
│   └── frontend/       # Frontend application code
├── tests/
│   ├── unit/          # Unit tests
│   ├── integration/   # Integration tests
│   └── e2e/           # End-to-end tests
├── config/
│   ├── dev/           # Development configuration
│   ├── test/          # Test configuration
│   └── prod/          # Production configuration
├── infra/             # Infrastructure as Code (Bicep/Terraform)
├── docs/              # Comprehensive documentation
│   ├── architecture/  # Architecture documentation
│   ├── api/          # API documentation
│   └── guides/       # User guides and tutorials
├── specs/             # Product and technical specifications
│   ├── prd.md        # Product Requirements Document
│   ├── features/     # Feature Requirements Documents
│   └── tasks/        # Technical Task Specifications
├── .github/           # GitHub workflows and configurations
└── scripts/          # Build and deployment scripts
```

## 🚀 Getting Started

### Prerequisites

- **Node.js** >= 18.0.0
- **Python** >= 3.12
- **Git**
- **Azure CLI** (for deployment)
- **Docker** (optional, for containerized development)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/erydrn/marketing-agent.git
   cd marketing-agent
   ```

2. **Set up environment**
   ```bash
   cp .env.template .env
   # Edit .env with your configuration
   ```

3. **Install dependencies**

   Backend (Python):
   ```bash
   pip install -r requirements-dev.txt
   ```

   Frontend (Node.js):
   ```bash
   npm install
   ```

4. **Set up pre-commit hooks** (optional but recommended)
   ```bash
   pre-commit install
   ```

### Development

```bash
# Backend development
# Commands will be added based on chosen framework

# Frontend development
npm run dev

# Run tests
npm test              # All tests
npm run test:unit    # Unit tests only
npm run test:e2e     # E2E tests only

# Linting and formatting
npm run lint
npm run format

# Documentation
npm run docs:serve   # Serve docs locally at http://localhost:8000
npm run docs:build   # Build static documentation
```

## 📖 Documentation

- **[Product Requirements (PRD)](specs/prd.md)** - Product vision, goals, and requirements
- **[Features](specs/features/)** - Detailed feature specifications
- **[Tasks](specs/tasks/)** - Technical implementation tasks
- **[Architecture](docs/architecture/)** - System architecture and design
- **[API Documentation](docs/api/)** - API reference and integration guides
- **[User Guides](docs/guides/)** - Tutorials and how-to documentation
- **[Contributing Guide](CONTRIBUTING.md)** - How to contribute to the project
- **[Changelog](CHANGELOG.md)** - Version history and changes

### Generating Documentation

This project uses [MkDocs](https://www.mkdocs.org/) with the Material theme:

```bash
# Install mkdocs if not already installed
pip install mkdocs mkdocs-material

# Serve locally
mkdocs serve

# Build static site
mkdocs build
```

## 🧪 Testing

### Running Tests

```bash
# Run all tests
npm test

# Run specific test suites
npm run test:unit          # Unit tests
npm run test:integration   # Integration tests
npm run test:e2e           # End-to-end tests

# Run with coverage
npm run test:coverage
```

### Test Structure

- **Unit Tests** (`tests/unit/`) - Test individual components in isolation
- **Integration Tests** (`tests/integration/`) - Test component interactions
- **E2E Tests** (`tests/e2e/`) - Test complete user workflows

See [Testing Documentation](tests/README.md) for detailed testing guidelines.

## 🔧 Configuration

Configuration is managed through environment variables and configuration files:

- **`.env`** - Local environment variables (not committed)
- **`.env.template`** - Template with required variables
- **`config/`** - Environment-specific configuration files

### Environment Variables

Key environment variables (see `.env.template` for complete list):

```bash
APP_ENV=development
DATABASE_URL=sqlite:///./dev.db
SECRET_KEY=your-secret-key-here
AZURE_KEY_VAULT_NAME=your-keyvault-name
```

⚠️ **Never commit secrets or sensitive data to version control**

## 🚢 Deployment

Deployment is automated using Azure Developer CLI and GitHub Actions.

### Prerequisites

- Azure subscription
- Azure CLI installed
- Appropriate Azure permissions

### Deploy to Azure

```bash
# Login to Azure
az login

# Initialize Azure Developer CLI
azd init

# Deploy infrastructure and application
azd up
```

See [Infrastructure Documentation](infra/README.md) for detailed deployment instructions.

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details on:

- Code of conduct
- Development workflow
- Coding standards
- Pull request process
- Testing requirements

### Quick Contribution Steps

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Run tests and linting
5. Commit your changes (`git commit -m 'feat: add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## 📊 Project Status

**Current Phase:** Foundation & Scaffolding

See [Implementation Roadmap](specs/tasks/000-IMPLEMENTATION-ROADMAP.md) for detailed project timeline.

### Completed
- ✅ Product Requirements Document (PRD)
- ✅ Feature Requirements Documents (FRDs)
- ✅ Technical Task Breakdown
- ✅ Project Scaffolding (In Progress)

### In Progress
- 🚧 Project Foundation Setup
- 🚧 Backend API Scaffolding
- 🚧 Data Persistence Layer

### Upcoming
- ⏳ Lead Capture API
- ⏳ Lead Qualification Engine
- ⏳ Analytics Dashboard
- ⏳ Sales Agent Integration

## 📜 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## 🔗 Related Resources

- [Spec2Cloud Workflow](SPEC2CLOUD.md) - AI-powered development workflow
- [Integration Guide](INTEGRATION.md) - How spec2cloud integrates with this project
- [APM Configuration](apm.yml) - Agent Package Manager configuration

## 📞 Support

- **Issues:** [GitHub Issues](https://github.com/erydrn/marketing-agent/issues)
- **Discussions:** [GitHub Discussions](https://github.com/erydrn/marketing-agent/discussions)

## 🙏 Acknowledgments

This project uses the [spec2cloud](https://github.com/EmeaAppGbb/spec2cloud) workflow for AI-powered development, featuring specialized GitHub Copilot agents for product management, development, and Azure deployment.

For more information about the spec2cloud workflow used to build this project, see [SPEC2CLOUD.md](SPEC2CLOUD.md).

---

**Built with ❤️ using spec2cloud and AI-powered development workflows**

