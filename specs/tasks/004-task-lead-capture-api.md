# Task 004: Multi-Channel Lead Capture - API Endpoints

**Feature:** FRD-001 (Lead Capture & Multi-Channel Integration)  
**Priority:** P0 (Critical)  
**Estimated Complexity:** High  
**Dependencies:** 002-task-backend-api-scaffolding.md, 003-task-data-persistence-layer.md

---

## Description

Implement REST API endpoints for capturing leads from multiple channels including web forms, ad platforms (via webhooks), partner referrals, developer uploads, and PR events. Each endpoint validates incoming data, performs initial data quality checks, and stores leads in the database.

This task implements the core lead ingestion capability that all other features depend on.

---

## Technical Requirements

### API Endpoints

#### POST /api/v1/leads/web-form
**Purpose:** Capture leads from website contact forms and landing pages  
**Authentication:** API key or public endpoint with CAPTCHA verification  
**Request Body:**
```json
{
  "source": "contact-form" | "quote-request" | "landing-page",
  "pageUrl": "string (URL)",
  "utmParams": {
    "source": "string (optional)",
    "medium": "string (optional)",
    "campaign": "string (optional)",
    "content": "string (optional)",
    "term": "string (optional)"
  },
  "contact": {
    "firstName": "string (required, 2-100 chars)",
    "lastName": "string (required, 2-100 chars)",
    "email": "string (required, valid email)",
    "phone": "string (optional, valid UK phone)",
    "preferredContactMethod": "email" | "phone" | "sms" (optional)
  },
  "property": {
    "address": "string (optional)",
    "postcode": "string (optional, valid UK postcode)",
    "propertyType": "detached" | "semi-detached" | "terraced" | "flat" | "other" (optional)
  },
  "serviceRequest": {
    "serviceType": "purchase" | "sale" | "remortgage" | "transfer" | "other",
    "timeline": "immediate" | "1-month" | "3-months" | "6-months" | "exploring" (optional),
    "message": "string (optional, max 5000 chars)"
  },
  "gdprConsent": "boolean (required)",
  "marketingConsent": "boolean (optional)",
  "referrer": "string (URL, optional)",
  "sessionData": {
    "pagesVisited": "array of URLs (optional)",
    "timeOnSite": "integer (seconds, optional)"
  }
}
```
**Response:** 201 Created with `{ "leadId": "uuid", "status": "captured" }`  
**Error Codes:** 400 (validation), 429 (rate limit), 500 (server error)

#### POST /api/v1/leads/ad-platform-webhook
**Purpose:** Receive lead data from ad platforms (Google Ads, Facebook, LinkedIn)  
**Authentication:** Webhook signature verification  
**Request Body:**
```json
{
  "platform": "google-ads" | "facebook" | "linkedin" | "microsoft-ads",
  "campaignId": "string",
  "campaignName": "string",
  "adGroupId": "string (optional)",
  "adId": "string (optional)",
  "formId": "string",
  "submittedAt": "ISO 8601 timestamp",
  "contact": {
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "phone": "string (optional)"
  },
  "customFields": "object (platform-specific fields)",
  "platformLeadId": "string (unique ID from platform)"
}
```
**Response:** 200 OK with `{ "leadId": "uuid", "status": "captured" }`  
**Error Codes:** 400 (validation), 401 (invalid signature), 500 (server error)

#### POST /api/v1/leads/partner-referral
**Purpose:** Capture leads from referral partners  
**Authentication:** Partner API key  
**Request Body:**
```json
{
  "partnerId": "string (required)",
  "partnerName": "string",
  "referralType": "introducer" | "developer" | "broker" | "other",
  "contact": {
    "firstName": "string (required)",
    "lastName": "string (required)",
    "email": "string (required)",
    "phone": "string (optional)"
  },
  "property": {
    "address": "string (optional)",
    "postcode": "string (optional)",
    "propertyType": "string (optional)",
    "estimatedValue": "number (optional)"
  },
  "serviceRequest": {
    "serviceType": "string (required)",
    "timeline": "string (optional)",
    "notes": "string (optional)"
  },
  "referralAgreement": {
    "commissionRate": "number (optional)",
    "commissionType": "fixed" | "percentage" (optional)"
  }
}
```
**Response:** 201 Created with `{ "leadId": "uuid", "status": "captured", "partnerConfirmation": "string" }`

#### POST /api/v1/leads/developer-bulk
**Purpose:** Bulk upload leads from property developers  
**Authentication:** Developer API key  
**Request Body:**
```json
{
  "developerId": "string (required)",
  "developmentName": "string (required)",
  "developmentLocation": "string",
  "leads": [
    {
      "contact": { /* same as above */ },
      "property": {
        "developmentUnit": "string",
        "plotNumber": "string",
        "propertyType": "string",
        "price": "number",
        "purchaseStage": "reserved" | "exchanged" | "completed"
      },
      "serviceRequest": { /* same as above */ }
    }
  ]
}
```
**Response:** 200 OK with `{ "processed": number, "successful": number, "failed": number, "leadIds": ["uuid"] }`  
**Validation:** Maximum 1000 leads per request

#### POST /api/v1/leads/event-registration
**Purpose:** Capture leads from PR events, webinars, seminars  
**Authentication:** API key  
**Request Body:**
```json
{
  "eventType": "webinar" | "seminar" | "exhibition" | "workshop",
  "eventName": "string (required)",
  "eventDate": "ISO 8601 date",
  "registrants": [
    {
      "contact": { /* same as above */ },
      "company": "string (optional)",
      "jobTitle": "string (optional)",
      "interests": "array of strings (optional)",
      "questions": "array of strings (optional)"
    }
  ]
}
```
**Response:** 200 OK with `{ "registered": number, "leadIds": ["uuid"] }`

### Data Validation Rules

**Contact Information:**
- Email: Must match RFC 5322 pattern, max 254 chars
- Phone: UK format validation (optional), normalize to E.164 format
- Name: 2-100 characters, no special characters except hyphen/apostrophe
- Required fields: firstName, lastName, email OR phone

**Property Information:**
- Postcode: UK format validation (AA9A 9AA or AA99 9AA or similar)
- Address: Max 500 characters
- Property type: From predefined enum only

**Service Request:**
- Service type: From predefined enum, required
- Timeline: From predefined enum, optional
- Message/notes: Max 5000 characters, sanitize HTML

**GDPR & Consent:**
- gdprConsent must be true (reject if false)
- marketingConsent is optional (defaults to false)
- Timestamp all consent records

### Duplicate Detection Logic
- Check for existing lead with same email + phone within 30 days
- Check for same email only within 30 days
- If duplicate found:
  - Append new submission data to existing lead record
  - Update `updated_at` timestamp
  - Create duplicate event log entry
  - Return existing leadId with status "duplicate-merged"

### Data Enrichment
- Postcode lookup: Fetch full address from postcode (async, non-blocking)
- Phone formatting: Normalize to E.164 format
- Email validation: Check against disposable email domains
- Geographic assignment: Map postcode to region for routing

### Error Handling
- Validation errors: Return 400 with detailed field-level errors
- Duplicate email domains (typos): Suggest corrections
- Rate limiting: 100 requests/minute per IP, 1000/minute per API key
- Oversized payloads: Reject requests > 1MB

---

## Acceptance Criteria

- [ ] All 5 API endpoints are implemented and functional
- [ ] Request validation rejects invalid data with clear error messages
- [ ] All required fields are validated according to specification
- [ ] GDPR consent validation prevents capturing leads without consent
- [ ] Duplicate detection merges leads correctly (same email+phone)
- [ ] Data enrichment (postcode lookup, phone formatting) works correctly
- [ ] Webhook signature verification prevents unauthorized ad platform submissions
- [ ] Partner API key authentication works correctly
- [ ] Bulk upload endpoint handles up to 1000 leads efficiently
- [ ] Rate limiting is enforced correctly
- [ ] All successful captures return appropriate lead IDs
- [ ] All error scenarios return appropriate HTTP status codes and messages
- [ ] Lead data is persisted correctly in database
- [ ] Source attribution (channel, campaign, UTM params) is captured correctly

---

## Testing Requirements

### Unit Tests
- Test each endpoint's request validation logic
- Test duplicate detection algorithm with various scenarios
- Test data enrichment functions (postcode lookup, phone formatting)
- Test GDPR consent validation
- Test rate limiting logic
- Test webhook signature verification
- **Minimum Coverage:** 85% of endpoint code

### Integration Tests
- Test end-to-end lead capture from each endpoint
- Test database persistence of captured leads
- Test duplicate detection with real database queries
- Test concurrent submissions to same endpoint
- Test bulk upload with varying sizes (1, 100, 1000 leads)
- Test rate limiting enforcement under load
- Test error handling for database failures

### Contract Tests
- Validate OpenAPI specification matches implementation
- Test all documented request/response schemas
- Verify error response formats match specification
- Test that all required fields are enforced

### Performance Tests
- Web form endpoint: 100 requests/second sustained
- Webhook endpoint: 50 requests/second sustained
- Bulk upload: Process 1000 leads in < 10 seconds
- Response time: P95 < 500ms for single lead capture
- Database query time: < 100ms for duplicate detection

---

## Implementation Notes

**DO NOT Include:**
- Lead qualification logic (that's in Task 005)
- Sales Agent handoff (that's in Task 008)
- Analytics/reporting (that's in Task 009)
- Email notifications (comes later)

**DO Include:**
- Comprehensive input validation
- Detailed logging (capture source, validation errors, timing)
- Audit trail (who submitted, when, from where)
- Idempotency support (same request ID doesn't create duplicates)
- Clear API error messages for client debugging

**Best Practices:**
- Use transaction boundaries for database writes
- Implement request ID generation for traceability
- Log all failed validation attempts (potential attack detection)
- Sanitize all text inputs to prevent XSS
- Use parameterized queries to prevent SQL injection
- Implement circuit breaker for external enrichment services

---

## Related Documents

- FRD-001: [specs/features/lead-capture-integration.md](../features/lead-capture-integration.md) - Full requirements
- FRD-002: [specs/features/lead-qualification-routing.md](../features/lead-qualification-routing.md) - Downstream consumer
- PRD [REQ-1]: Multi-source lead capture requirements
- PRD [REQ-4]: Data quality validation requirements
- PRD [REQ-9]: GDPR compliance requirements

---

## Definition of Done

- [ ] All acceptance criteria met
- [ ] All unit tests pass with ≥85% coverage
- [ ] All integration tests pass
- [ ] All contract tests pass
- [ ] Performance tests meet requirements
- [ ] OpenAPI specification updated with all endpoints
- [ ] API documentation includes examples for each endpoint
- [ ] Code review completed
- [ ] Security review completed (no vulnerabilities)
- [ ] Load testing completed (meets PRD requirement: capture within 5 seconds)
