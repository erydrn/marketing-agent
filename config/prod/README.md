# Production Configuration

Configuration files for production environment.

## Characteristics

- Production database connections
- Real external service integrations
- Structured logging (JSON)
- Performance optimizations
- Security hardening
- Error monitoring enabled

## Security Notes

⚠️ **All sensitive values must be stored in Azure Key Vault or equivalent**

Production configuration files should only contain:
- Structure and schema
- Non-sensitive defaults
- References to secret storage

**Placeholder:** Configuration files will be added during deployment setup.
