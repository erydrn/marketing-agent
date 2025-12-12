# Implementation Roadmap - Digital Marketing Agent

**Version:** 1.0  
**Last Updated:** December 12, 2025  
**Status:** Planning

---

## Overview

This document provides a prioritized, sequenced list of implementation tasks for the Digital Marketing Agent, organized by phase and dependency order. Tasks are derived from the 4 FRDs and follow the critical path identified in the traceability matrix.

---

## Implementation Strategy

### Approach: Incremental Delivery
- **Phase 1:** Foundation + Core Lead Capture (FRD-001)
- **Phase 2:** Lead Qualification & Sales Integration (FRD-002 + FRD-004)
- **Phase 3:** Analytics & Optimization (FRD-003)

### Guiding Principles
1. **Start Simple:** In-memory storage before databases, file-based before cloud services
2. **Vertical Slices:** Each phase delivers end-to-end value
3. **Test Early:** Integration testing at each milestone
4. **Defer Complexity:** Advanced features (ML scoring, predictive analytics) in Phase 3+

---

## Phase 1: Foundation & Lead Capture (Weeks 1-4)

**Goal:** Capture leads from primary channels and store them with basic validation

### 1.1 Project Foundation (Week 1)

#### Task 001: Project Scaffolding
**Priority:** P0 - Blocker  
**Depends On:** None  
**Estimated Effort:** 2 days

**Deliverables:**
- Project structure (src/, specs/, tests/, docs/)
- Build configuration (package.json, tsconfig.json, or equivalent)
- Linting and code quality tools
- Git repository setup with .gitignore
- CI/CD pipeline skeleton (GitHub Actions or Azure DevOps)

**Acceptance Criteria:**
- Project builds successfully
- Linter runs without errors
- CI pipeline executes (even if minimal)

---

#### Task 002: Technology Stack Selection
**Priority:** P0 - Blocker  
**Depends On:** Task 001  
**Estimated Effort:** 1 day

**Decisions Required:**
- Backend framework (Node.js/Express, .NET/ASP.NET Core, Python/FastAPI)
- Database (PostgreSQL, MongoDB, Azure SQL, or start with file-based)
- API architecture (REST, GraphQL)
- Authentication method (API keys, OAuth 2.0)
- Deployment target (Azure App Service, Functions, Container Apps)

**Deliverables:**
- Architecture Decision Record (ADR) documenting choices
- Starter code with chosen stack

**Acceptance Criteria:**
- ADR created in specs/adr/
- Hello World endpoint responds

---

#### Task 003: Data Models & Schema
**Priority:** P0 - Blocker  
**Depends On:** Task 002  
**Estimated Effort:** 3 days

**Deliverables:**
- Lead data model (TypeScript interfaces, C# classes, or Python models)
- Source attribution model
- Validation schema (Zod, Joi, FluentValidation, Pydantic)
- Database schema (if using database) or JSON storage structure
- Migration scripts

**Acceptance Criteria:**
- Lead can be created with required fields
- Validation rejects invalid data
- Database migrations run successfully (if applicable)

---

### 1.2 Core Lead Capture (Weeks 2-3)

#### Task 004: Lead Capture API
**Priority:** P0 - Critical  
**Depends On:** Task 003  
**Estimated Effort:** 3 days

**Deliverables:**
- POST /api/leads endpoint
- Request validation
- Lead storage (database or file)
- API documentation (Swagger/OpenAPI)
- Unit tests

**Acceptance Criteria:**
- Endpoint accepts valid lead JSON
- Returns 201 Created with lead ID
- Returns 400 Bad Request for invalid data
- Lead persists and can be retrieved
- 95% test coverage

---

#### Task 005: Website Form Integration
**Priority:** P0 - Critical  
**Depends On:** Task 004  
**Estimated Effort:** 3 days

**Deliverables:**
- Form submission handler (HTML form or API client)
- CORS configuration
- UTM parameter capture
- Session tracking (optional)
- Form validation (client + server)

**Acceptance Criteria:**
- Form submits successfully
- Lead appears in system within 5 seconds
- UTM parameters preserved
- Duplicate submissions detected

---

#### Task 006: Google Ads Lead Forms Integration
**Priority:** P1 - High  
**Depends On:** Task 004  
**Estimated Effort:** 4 days

**Deliverables:**
- Google Ads API client setup
- Webhook receiver for lead form submissions
- Field mapping (Google fields → Lead model)
- OAuth 2.0 authentication
- Error handling and retry logic

**Acceptance Criteria:**
- Lead form submission captured within 60 seconds
- All standard fields mapped correctly
- Failed captures retry 3 times
- Alert sent on persistent failures

---

#### Task 007: Basic Data Validation & Enrichment
**Priority:** P1 - High  
**Depends On:** Task 004  
**Estimated Effort:** 3 days

**Deliverables:**
- Email validation (format, disposable domain check)
- Phone number validation and formatting (UK)
- Postcode lookup integration (Royal Mail PAF or Google)
- Required field validation
- Duplicate detection (email + phone)

**Acceptance Criteria:**
- Invalid emails rejected
- Phone numbers formatted to +44 standard
- Postcode resolves to full address
- Duplicates flagged (not rejected)

---

#### Task 008: Partner Referral Email Capture
**Priority:** P1 - High  
**Depends On:** Task 004  
**Estimated Effort:** 4 days

**Deliverables:**
- Email forwarding integration
- Email parsing service (extract lead data from email body)
- Partner identification from sender
- Manual review queue for unparseable emails

**Acceptance Criteria:**
- Forwarded email creates lead record
- Partner attribution preserved
- Parsing accuracy >90% on test emails
- Failed parses go to review queue

---

### 1.3 Phase 1 Integration & Testing (Week 4)

#### Task 009: Lead Storage & Retrieval
**Priority:** P0 - Critical  
**Depends On:** Tasks 004-008  
**Estimated Effort:** 2 days

**Deliverables:**
- GET /api/leads/:id endpoint
- GET /api/leads (list with pagination)
- Filtering by source, channel, date range
- Sorting capabilities

**Acceptance Criteria:**
- Lead can be retrieved by ID
- List returns paginated results
- Filters work correctly
- Response time < 200ms for 10,000 leads

---

#### Task 010: Error Handling & Monitoring
**Priority:** P0 - Critical  
**Depends On:** Tasks 004-009  
**Estimated Effort:** 2 days

**Deliverables:**
- Centralized error handling middleware
- Structured logging (JSON logs)
- Health check endpoint
- Basic monitoring dashboard (Application Insights or similar)
- Alert configuration

**Acceptance Criteria:**
- Errors logged with context
- Health check returns system status
- Alerts sent for 5+ consecutive failures
- Log aggregation working

---

#### Task 011: Phase 1 End-to-End Testing
**Priority:** P0 - Critical  
**Depends On:** All Phase 1 tasks  
**Estimated Effort:** 3 days

**Deliverables:**
- Integration test suite (full capture flow)
- Performance tests (100 concurrent submissions)
- Security tests (SQL injection, XSS)
- Load testing report
- Bug fixes from testing

**Acceptance Criteria:**
- All integration tests pass
- System handles 100 leads/minute
- No critical security vulnerabilities
- < 5 second capture time under load

---

## Phase 2: Qualification & Sales Integration (Weeks 5-8)

**Goal:** Score, qualify, and route leads to Sales Agent with context

### 2.1 Lead Scoring & Qualification (Week 5)

#### Task 012: Lead Scoring Engine
**Priority:** P0 - Critical  
**Depends On:** Task 009  
**Estimated Effort:** 4 days

**Deliverables:**
- Scoring algorithm (completeness, engagement, readiness, source quality)
- Configurable scoring weights (config file or database)
- Score calculation service
- Tier assignment logic (Hot/Warm/Cool/Cold)
- Unit tests for scoring scenarios

**Acceptance Criteria:**
- Score calculated within 2 seconds
- Same inputs = same score (idempotent)
- Tiers assigned correctly
- Scoring weights can be updated without code changes

---

#### Task 013: Qualification Rules Engine
**Priority:** P0 - Critical  
**Depends On:** Task 012  
**Estimated Effort:** 3 days

**Deliverables:**
- Geographic qualification (UK postcode validation)
- Service type matching
- Exclusion list checking (competitor domains)
- Qualification decision logic
- Disqualification reason tracking

**Acceptance Criteria:**
- Qualification decision made within 3 seconds
- Disqualified leads go to holding queue
- Disqualification reasons logged
- Edge cases flagged for manual review

---

#### Task 014: Lead Enrichment Service
**Priority:** P1 - High  
**Depends On:** Task 013  
**Estimated Effort:** 3 days

**Deliverables:**
- Property data enrichment (value estimation, area data)
- Behavioral context aggregation
- Recommended actions generation
- Talking points extraction
- Next steps suggestions

**Acceptance Criteria:**
- Enrichment completes within 10 seconds
- Missing data doesn't block routing
- Enrichment sources documented
- Privacy compliance maintained

---

### 2.2 Routing & Queue Management (Week 6)

#### Task 015: Routing Engine
**Priority:** P0 - Critical  
**Depends On:** Task 013  
**Estimated Effort:** 4 days

**Deliverables:**
- Routing decision logic (Sales, New Business, Nurture, Holding)
- Priority calculation (Critical/High/Standard/Low)
- Queue management system
- SLA deadline calculation
- Routing status tracking

**Acceptance Criteria:**
- Routing decision within 5 seconds
- Leads routed to correct destination 100%
- Priority visible immediately
- SLA timers start correctly

---

#### Task 016: Re-engagement Detection
**Priority:** P1 - High  
**Depends On:** Task 015  
**Estimated Effort:** 3 days

**Deliverables:**
- Duplicate detection logic (30-day window)
- Re-engagement scoring boost
- Previous interaction history retrieval
- Multi-channel attribution tracking

**Acceptance Criteria:**
- Duplicates detected within 2 seconds
- No duplicate Sales Agent tasks created
- History displayed with new touchpoint
- Multi-touch attribution preserved

---

### 2.3 Sales Agent Integration (Week 7)

#### Task 017: Sales Agent API Client
**Priority:** P0 - Critical  
**Depends On:** Task 015  
**Estimated Effort:** 4 days

**Deliverables:**
- HTTP client for Sales Agent API
- Lead handoff payload construction
- OAuth 2.0 or API key authentication
- Retry logic with exponential backoff
- Acknowledgment handling

**Acceptance Criteria:**
- Lead transmitted within 5 seconds
- Acknowledgment received within 10 seconds
- 3 automatic retries on failure
- Failed transmissions queued

---

#### Task 018: Conversion Feedback Webhook
**Priority:** P1 - High  
**Depends On:** Task 017  
**Estimated Effort:** 3 days

**Deliverables:**
- Webhook receiver endpoint
- Signature verification
- Feedback event processing (contact, qualification, opportunity, case)
- Lead status updates
- Conversion tracking

**Acceptance Criteria:**
- Webhook receives and validates events
- Lead status updated within 15 minutes
- 100% of conversions tracked
- Invalid webhooks rejected

---

### 2.4 Portal Integrations (Week 8)

#### Task 019: Lead System Integration
**Priority:** P0 - Critical  
**Depends On:** Task 017  
**Estimated Effort:** 4 days

**Deliverables:**
- Lead System database write access OR API integration
- Lead data synchronization
- Priority queue display
- Bidirectional status updates

**Acceptance Criteria:**
- Lead appears in portal within 30 seconds
- Status updates sync both ways
- No data loss during sync
- Conflict resolution works

---

#### Task 020: Digital Quoting Integration
**Priority:** P1 - High  
**Depends On:** Task 019  
**Estimated Effort:** 3 days

**Deliverables:**
- Quoting system API integration
- Pre-populated quote generation
- Quote status tracking
- Quote engagement tracking

**Acceptance Criteria:**
- Quote pre-filled with lead data
- Quote sent automatically (for standard cases)
- Quote status flows back to Marketing Agent
- Email tracking works

---

#### Task 021: Introducer Portal Integration
**Priority:** P2 - Medium  
**Depends On:** Task 019  
**Estimated Effort:** 3 days

**Deliverables:**
- Partner dashboard integration
- Referral status visibility
- Commission tracking link
- Performance metrics display

**Acceptance Criteria:**
- Partners see their referred leads
- Status updates in real-time
- Commission calculations correct
- Performance metrics accurate

---

#### Task 022: Phase 2 Integration Testing
**Priority:** P0 - Critical  
**Depends On:** All Phase 2 tasks  
**Estimated Effort:** 3 days

**Deliverables:**
- End-to-end qualification tests
- Sales Agent integration tests
- Portal sync tests
- Performance tests
- Bug fixes

**Acceptance Criteria:**
- All E2E tests pass
- Integration with Sales Agent stable
- Portal sync reliable
- Performance targets met

---

## Phase 3: Analytics & Reporting (Weeks 9-11)

**Goal:** Provide visibility into marketing performance and ROI

### 3.1 Data Pipeline & Warehouse (Week 9)

#### Task 023: Analytics Data Pipeline
**Priority:** P1 - High  
**Depends On:** Tasks 009, 017  
**Estimated Effort:** 4 days

**Deliverables:**
- Data extraction from lead database
- Data transformation for analytics
- Aggregation logic (daily, weekly, monthly)
- Data warehouse schema (or reporting database)
- ETL job scheduling

**Acceptance Criteria:**
- Daily aggregations run automatically
- Data accurate compared to source
- Historical data preserved
- Query performance < 3 seconds

---

#### Task 024: External Data Integrations
**Priority:** P1 - High  
**Depends On:** Task 023  
**Estimated Effort:** 3 days

**Deliverables:**
- Google Ads API integration (spend, impressions, clicks)
- Facebook/Meta Ads API integration
- LinkedIn Ads API integration
- Website analytics integration (GA4 or similar)
- Financial data integration (budget, spend)

**Acceptance Criteria:**
- Ad platform data synced daily
- Spend data accurate
- API rate limits respected
- Missing data handled gracefully

---

### 3.2 Real-Time Dashboard (Week 10)

#### Task 025: Dashboard Backend API
**Priority:** P1 - High  
**Depends On:** Task 023  
**Estimated Effort:** 3 days

**Deliverables:**
- Dashboard metrics API endpoints
- Real-time data aggregation
- WebSocket or polling support
- Caching layer (Redis or in-memory)
- Query optimization

**Acceptance Criteria:**
- Dashboard data loads in < 3 seconds
- Data refreshes every 30 seconds
- Supports 50 concurrent users
- Cache hit rate > 80%

---

#### Task 026: Dashboard Frontend
**Priority:** P1 - High  
**Depends On:** Task 025  
**Estimated Effort:** 5 days

**Deliverables:**
- React/Vue/Angular dashboard UI
- Lead flow visualization
- Channel performance charts
- Alert panel
- Quick stats widgets
- Mobile responsive design

**Acceptance Criteria:**
- Dashboard renders in < 2 seconds
- Charts update in real-time
- Mobile-friendly
- Accessible (WCAG 2.1 AA)

---

### 3.3 Reports & Analytics (Week 11)

#### Task 027: Channel Attribution Reports
**Priority:** P1 - High  
**Depends On:** Task 023, 024  
**Estimated Effort:** 4 days

**Deliverables:**
- Channel performance report
- Sub-channel drill-down
- Campaign-level attribution
- ROI calculations
- Conversion funnel report
- Export functionality (CSV, Excel, PDF)

**Acceptance Criteria:**
- Reports generate in < 10 seconds
- All data exportable
- Filters work correctly
- ROI calculations validated

---

#### Task 028: Partner & Developer Dashboards
**Priority:** P2 - Medium  
**Depends On:** Task 027  
**Estimated Effort:** 3 days

**Deliverables:**
- Partner performance dashboard
- Developer lead analytics
- Performance rankings
- Revenue attribution
- Commission reports

**Acceptance Criteria:**
- Partner-specific views work
- Performance metrics accurate
- Rankings calculated correctly
- Commission totals match expectations

---

#### Task 029: Scheduled Reports
**Priority:** P2 - Medium  
**Depends On:** Task 027  
**Estimated Effort:** 3 days

**Deliverables:**
- Report scheduler (cron or task queue)
- Email delivery service integration
- Daily summary reports
- Weekly performance reports
- Monthly executive summaries
- Report templates

**Acceptance Criteria:**
- Reports auto-generate on schedule
- Emails delivered reliably
- Recipients configurable
- PDF/Excel formats supported

---

#### Task 030: Phase 3 Testing & Optimization
**Priority:** P1 - High  
**Depends On:** All Phase 3 tasks  
**Estimated Effort:** 3 days

**Deliverables:**
- Analytics accuracy validation
- Dashboard performance testing
- Report generation testing
- Data integrity checks
- Optimization based on findings

**Acceptance Criteria:**
- Analytics match source data
- Dashboard handles 50+ users
- Reports accurate
- No performance bottlenecks

---

## Phase 4: Enhancement & Optimization (Weeks 12+)

**Goal:** Advanced features and continuous improvement

### Optional Enhancements (Prioritize based on feedback)

#### Task 031: Lead Nurture Campaign Integration
**Priority:** P2 - Medium  
**Estimated Effort:** 5 days

**Deliverables:**
- Email marketing platform integration
- Nurture campaign assignment logic
- Email engagement tracking
- Re-engagement triggers

---

#### Task 032: A/B Testing Framework
**Priority:** P2 - Medium  
**Estimated Effort:** 4 days

**Deliverables:**
- Experiment configuration
- Variant assignment
- Statistical significance calculation
- Winner declaration logic

---

#### Task 033: Machine Learning Lead Scoring
**Priority:** P3 - Low  
**Estimated Effort:** 10 days

**Deliverables:**
- ML model training pipeline
- Feature engineering
- Model deployment
- Score prediction service
- Model monitoring

---

#### Task 034: Predictive Analytics
**Priority:** P3 - Low  
**Estimated Effort:** 8 days

**Deliverables:**
- Conversion probability prediction
- Lead volume forecasting
- Anomaly detection
- Trend analysis

---

#### Task 035: Advanced Security & Compliance
**Priority:** P2 - Medium  
**Estimated Effort:** 5 days

**Deliverables:**
- Data encryption at rest
- Advanced audit logging
- Right to erasure automation
- Compliance reporting

---

## Critical Path Summary

```
Week 1-2:   Foundation → Lead Capture API → Website Forms
Week 3-4:   Google Ads → Partner Email → E2E Testing [MILESTONE 1: Basic Capture]
Week 5-6:   Scoring → Qualification → Routing [MILESTONE 2: Intelligent Routing]
Week 7-8:   Sales Integration → Portals → Testing [MILESTONE 3: Sales Handoff]
Week 9-10:  Analytics Pipeline → Dashboard [MILESTONE 4: Visibility]
Week 11:    Reports → Testing [MILESTONE 5: Full Analytics]
Week 12+:   Enhancements [MILESTONE 6: Optimization]
```

**Minimum Viable Product (MVP):** Milestones 1-3 (Weeks 1-8)  
**Full Feature Set:** Milestones 1-5 (Weeks 1-11)  
**Advanced Features:** Milestone 6 (Ongoing)

---

## Risk Mitigation

### High-Risk Dependencies
1. **Sales Agent API availability** - Mitigate: Build with mock API, switch to real later
2. **Ad platform API access** - Mitigate: Use test accounts, get production access early
3. **Postcode lookup service** - Mitigate: Start with basic validation, add lookup later

### Technical Risks
1. **Performance at scale** - Mitigate: Load test early, optimize continuously
2. **Data quality issues** - Mitigate: Extensive validation, manual review queue
3. **Integration failures** - Mitigate: Retry logic, queue-based architecture, monitoring

---

## Success Criteria by Phase

### Phase 1 Success
- ✅ 100 leads/day captured from website and Google Ads
- ✅ < 5 second capture time
- ✅ 99% capture success rate
- ✅ Basic validation working

### Phase 2 Success
- ✅ Lead scoring accuracy validated (90%+ of Hot leads convert)
- ✅ Sales Agent receives 95%+ of qualified leads within SLA
- ✅ Portal integration stable
- ✅ Re-engagement detection working

### Phase 3 Success
- ✅ Dashboard available 99.9% uptime
- ✅ ROI reporting accurate
- ✅ Marketing team using reports daily
- ✅ Channel attribution complete

---

## Resource Requirements

### Team Composition (Suggested)
- **Backend Developer:** 1 FTE (Weeks 1-11)
- **Frontend Developer:** 0.5 FTE (Weeks 10-11)
- **DevOps/Platform Engineer:** 0.25 FTE (ongoing)
- **QA Engineer:** 0.5 FTE (Weeks 4, 8, 11)
- **Product Manager:** 0.25 FTE (ongoing)

### Infrastructure
- Development environment
- Staging environment (mimics production)
- Production environment (Azure recommended)
- CI/CD pipeline
- Monitoring and logging services

---

## Next Steps

1. **Review and approve** this roadmap with stakeholders
2. **Select technology stack** (Task 002) - Create ADR
3. **Set up development environment** (Task 001)
4. **Begin Phase 1 implementation**
5. **Weekly progress reviews** against this plan

---

**Document Owner:** Planner Agent  
**Approved By:** [Pending]  
**Next Review:** End of Week 2 (adjust based on actual progress)
