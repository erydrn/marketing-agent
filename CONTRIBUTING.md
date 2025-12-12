# Contributing to Digital Marketing Agent

Thank you for your interest in contributing to the Digital Marketing Agent! This document provides guidelines and instructions for contributing to the project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Testing Requirements](#testing-requirements)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Documentation](#documentation)

## Code of Conduct

- Be respectful and inclusive
- Focus on constructive feedback
- Help maintain a positive community
- Report any unacceptable behavior to project maintainers

## Getting Started

### Prerequisites

- Git
- Development environment tools (will be specified based on technology stack)
- Access to the repository

### Initial Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/erydrn/marketing-agent.git
   cd marketing-agent
   ```

2. **Create environment file**
   ```bash
   cp .env.template .env
   # Edit .env with your local configuration
   ```

3. **Install dependencies**
   ```bash
   # Commands will be added based on technology stack
   ```

4. **Verify setup**
   ```bash
   # Run tests to ensure everything works
   # Commands will be added based on testing framework
   ```

## Development Workflow

### Branching Strategy

- **`main`** - Production-ready code
- **`develop`** - Integration branch for features
- **`feature/*`** - New features (e.g., `feature/lead-capture-api`)
- **`bugfix/*`** - Bug fixes (e.g., `bugfix/database-connection`)
- **`hotfix/*`** - Urgent production fixes

### Creating a Feature Branch

```bash
git checkout develop
git pull origin develop
git checkout -b feature/your-feature-name
```

### Making Changes

1. Make your changes in small, logical commits
2. Write or update tests for your changes
3. Ensure all tests pass
4. Update documentation as needed
5. Follow coding standards (see below)

## Coding Standards

### General Principles

1. **Write Clean Code**
   - Self-documenting code with clear variable/function names
   - Keep functions small and focused
   - Follow DRY (Don't Repeat Yourself)
   - Use meaningful comments only when necessary

2. **Error Handling**
   - Always handle errors gracefully
   - Provide meaningful error messages
   - Log errors appropriately

3. **Security**
   - Never commit secrets or credentials
   - Validate all inputs
   - Follow security best practices
   - Use parameterized queries for databases

### Language-Specific Standards

Specific coding standards will be defined once the technology stack is selected. Refer to `AGENTS.md` for comprehensive guidelines.

### Code Formatting

- Use `.editorconfig` for consistent formatting across editors
- Run linters before committing (pre-commit hooks will enforce this)
- Follow established formatting conventions for the chosen language

## Testing Requirements

### Test Coverage

- **Minimum coverage:** 80% for new code
- Write tests before or alongside code (TDD encouraged)
- Include unit, integration, and e2e tests as appropriate

### Test Guidelines

1. **Unit Tests**
   - Test individual components in isolation
   - Mock external dependencies
   - Fast execution

2. **Integration Tests**
   - Test component interactions
   - Use test databases
   - Verify data flow

3. **E2E Tests**
   - Test critical user workflows
   - Run before releases
   - Keep focused on key scenarios

### Running Tests

```bash
# Commands will be added based on testing framework
# Example placeholders:
# npm test              # Run all tests
# npm run test:unit     # Run unit tests only
# npm run test:watch    # Run tests in watch mode
```

## Commit Guidelines

### Commit Message Format

Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- **feat:** New feature
- **fix:** Bug fix
- **docs:** Documentation changes
- **style:** Code style changes (formatting, etc.)
- **refactor:** Code refactoring
- **test:** Adding or updating tests
- **chore:** Maintenance tasks

### Examples

```
feat(lead-capture): add API endpoint for digital ads integration

Implement POST /api/v1/leads endpoint to capture leads from Google Ads
and Facebook campaigns. Includes validation, database persistence, and
Sales Agent notification.

Closes #123
```

```
fix(database): resolve connection pooling timeout issue

Update database pool configuration to prevent timeout errors under
high load. Increase pool size and add connection retry logic.

Fixes #125
```

## Pull Request Process

### Before Submitting

1. ✅ All tests pass
2. ✅ Code follows style guidelines
3. ✅ Documentation is updated
4. ✅ Commit messages follow guidelines
5. ✅ Branch is up to date with target branch
6. ✅ No merge conflicts

### PR Template

When creating a pull request, include:

- **Description:** What changes were made and why
- **Related Issues:** Link to issue(s) this PR addresses
- **Testing:** How the changes were tested
- **Screenshots:** For UI changes
- **Breaking Changes:** Note any breaking changes
- **Checklist:** Confirm all requirements are met

### Review Process

- At least one approval required before merging
- Address all review comments
- Keep PRs focused and reasonably sized
- Be responsive to feedback

### Merging

- Squash commits for cleaner history (when appropriate)
- Delete feature branch after merge
- Update CHANGELOG.md for significant changes

## Documentation

### Documentation Requirements

- Update relevant docs for any user-facing changes
- Add code comments for complex logic
- Update API documentation for endpoint changes
- Include examples in documentation

### Documentation Structure

- **README.md** - Project overview and setup
- **docs/** - Comprehensive documentation
- **CHANGELOG.md** - Track all changes
- **CONTRIBUTING.md** - This file
- **Code comments** - Inline documentation

## Questions or Issues?

- Check existing issues and documentation first
- Open a GitHub issue for bugs or feature requests
- Ask questions in discussions or pull requests

---

Thank you for contributing to the Digital Marketing Agent! 🚀
