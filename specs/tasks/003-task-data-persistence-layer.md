# Task 003: Data Storage & Persistence Layer Setup

**Feature:** Foundation  
**Priority:** P0 (Critical - Required Before Feature Development)  
**Estimated Complexity:** Medium  
**Dependencies:** 002-task-backend-api-scaffolding.md

---

## Description

Set up the data persistence layer including database configuration, connection management, ORM/data access framework, migration tooling, and data access patterns. This provides the foundation for all feature data storage needs.

Implements repository pattern for data access abstraction and establishes database schema management practices.

---

## Technical Requirements

### Database Selection & Configuration
- Configure database connection with environment-based settings
- Support connection pooling with configurable pool size
- Implement connection retry logic with exponential backoff
- Configure connection timeouts and keepalive settings
- Support read replicas for scaling (configuration only, not required initially)

### Connection Management
- Initialize database connection on application startup
- Implement graceful connection closure on shutdown
- Health check integration for database connectivity
- Connection pool monitoring and metrics
- Handle transient connection failures gracefully

### ORM/Data Access Framework
- Set up ORM or data access framework
- Configure entity/model mapping
- Set up query builder or query patterns
- Implement transaction management
- Configure lazy loading vs. eager loading strategies

### Schema Migration System
- Set up database migration tooling
- Create migration file structure and naming conventions
- Implement migration versioning
- Support rollback capabilities
- Automate migration execution in deployment pipeline

### Repository Pattern Implementation
- Create base repository interface/abstract class
- Define standard CRUD operations (Create, Read, Update, Delete)
- Implement query specifications or criteria patterns
- Support pagination and filtering
- Implement optimistic concurrency control (version tracking)

### Data Models - Core Entities
**Lead Entity:**
- Unique identifier (UUID or auto-increment)
- External lead ID (from marketing system)
- Contact information (name, email, phone)
- Property details
- Service request information
- Timestamps (created_at, updated_at)
- Soft delete support (deleted_at)
- Version/concurrency token

**Lead Score Entity:**
- Associated lead ID (foreign key)
- Overall score (0-100)
- Tier classification (Hot/Warm/Cool/Cold)
- Component scores (completeness, engagement, readiness, source quality)
- Calculated timestamp

**Lead Source Attribution:**
- Associated lead ID
- Channel, source, campaign, medium
- UTM parameters
- Referrer information
- Landing page URL
- Timestamp

### Data Access Patterns
- Implement Unit of Work pattern for transaction management
- Create specifications for complex queries
- Implement result pagination helpers
- Add query performance optimization (indexes, query hints)
- Implement caching layer hooks (interface, no implementation yet)

### Database Seeding
- Create seed data for development and testing
- Implement data factory/builders for test data
- Support environment-specific seed data
- Automate seed data loading for new environments

---

## Acceptance Criteria

- [ ] Database connection establishes successfully on startup
- [ ] Connection pool is configured and monitored
- [ ] Health check reflects database connectivity status
- [ ] ORM/data access framework is configured and functional
- [ ] Migration system can create, run, and rollback migrations
- [ ] Base repository pattern is implemented with standard CRUD operations
- [ ] Core data models (Lead, LeadScore, LeadSourceAttribution) are defined
- [ ] Transactions can be initiated, committed, and rolled back
- [ ] Database connection closes gracefully on application shutdown
- [ ] Seed data can be loaded for development environments
- [ ] Query performance monitoring is in place
- [ ] Concurrency conflicts are detected and handled appropriately

---

## Testing Requirements

### Unit Tests
- Test repository CRUD operations with in-memory or test database
- Test transaction commit and rollback scenarios
- Test concurrency conflict detection
- Test query specification building
- Test pagination logic
- **Minimum Coverage:** 85% of data access code

### Integration Tests
- Test actual database connection and disconnection
- Test migration execution (up and down)
- Test seed data loading
- Test repository operations against real database
- Test transaction isolation levels
- Test connection pool under load
- Test connection retry on transient failures

### Performance Tests
- Measure query execution time for common operations
- Test connection pool performance under concurrent load
- Validate index effectiveness on key queries
- Test bulk insert/update performance

---

## Database Schema (Entities Only - No SQL)

### leads Table
- `id` - Primary key (UUID or BigInt)
- `external_lead_id` - String (unique, indexed)
- `created_at` - Timestamp
- `updated_at` - Timestamp
- `deleted_at` - Timestamp (nullable, for soft deletes)
- `version` - Integer (for optimistic locking)
- `first_name` - String
- `last_name` - String
- `email` - String (indexed)
- `phone` - String
- `address` - Text
- `postcode` - String (indexed)
- `property_type` - Enum/String
- `service_type` - Enum/String
- `timeline` - Enum/String
- `urgency` - Enum/String

### lead_scores Table
- `id` - Primary key
- `lead_id` - Foreign key to leads (indexed)
- `overall_score` - Integer (0-100)
- `tier` - Enum (Hot/Warm/Cool/Cold)
- `completeness_score` - Integer (0-25)
- `engagement_score` - Integer (0-25)
- `readiness_score` - Integer (0-25)
- `source_quality_score` - Integer (0-25)
- `calculated_at` - Timestamp

### lead_source_attributions Table
- `id` - Primary key
- `lead_id` - Foreign key to leads (indexed)
- `channel` - String (indexed)
- `source` - String (indexed)
- `campaign` - String
- `medium` - String
- `utm_source` - String
- `utm_medium` - String
- `utm_campaign` - String
- `utm_content` - String
- `utm_term` - String
- `referrer` - Text
- `landing_page` - Text
- `captured_at` - Timestamp

### Indexes
- `leads.email` for duplicate detection
- `leads.external_lead_id` for lookup
- `leads.postcode` for geographic queries
- `lead_scores.lead_id` for joins
- `lead_source_attributions.lead_id` for joins
- `lead_source_attributions.channel` for reporting

---

## Implementation Notes

**DO NOT Include:**
- Specific database vendor lock-in (keep it abstracted through ORM)
- Complex business logic in repositories (keep it in service layer)
- Feature-specific database tables (those come with feature tasks)
- Authentication or user management tables (separate concern)

**DO Include:**
- Clear separation between data access and business logic
- Database-agnostic patterns where possible
- Comprehensive migration examples
- Performance considerations (indexing strategy)
- Data integrity constraints (foreign keys, unique constraints)

**Best Practices:**
- Use parameterized queries to prevent SQL injection
- Implement database connection pooling for performance
- Use migrations for all schema changes (never manual DDL)
- Keep models/entities focused (single responsibility)
- Document all schema decisions in migration files

---

## Related Documents

- FRD-001: [specs/features/lead-capture-integration.md](../features/lead-capture-integration.md) - Defines lead data requirements
- FRD-002: [specs/features/lead-qualification-routing.md](../features/lead-qualification-routing.md) - Defines scoring data requirements
- FRD-003: [specs/features/analytics-reporting.md](../features/analytics-reporting.md) - Defines reporting data access needs
- FRD-004: [specs/features/sales-agent-integration.md](../features/sales-agent-integration.md) - Defines lead handoff data requirements

---

## Definition of Done

- [ ] All acceptance criteria met
- [ ] All unit tests pass with ≥85% coverage
- [ ] All integration tests pass
- [ ] All migrations execute successfully (up and down)
- [ ] Seed data loads correctly in development environment
- [ ] Database health check integrated with API health endpoint
- [ ] Code review completed
- [ ] Database connection performance verified (handles 100 concurrent connections)
- [ ] Query performance meets requirements (< 100ms for standard queries)
- [ ] Database schema documented with ER diagram
