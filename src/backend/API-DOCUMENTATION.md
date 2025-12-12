# Marketing Agent API Documentation

## Base URL
- Development: `http://localhost:5195` or `https://localhost:7195`
- Production: TBD

## Authentication
Currently, the API uses InMemory database and no authentication. In production:
- API Key authentication via `X-API-Key` header
- Webhook signature verification for ad platform webhooks

## Endpoints

### Health Check

#### GET /health
Check if the API is running.

**Response:**
```json
{
  "status": "ok",
  "timestamp": "2025-12-12T11:23:00.000Z"
}
```

---

### Lead Capture Endpoints

#### POST /api/v1/leads/web-form
Capture a lead from a website form submission.

**Request Body:**
```json
{
  "source": "contact-form | quote-request | landing-page",
  "pageUrl": "https://example.com/contact",
  "utmParams": {
    "source": "google",
    "medium": "cpc",
    "campaign": "spring-sale",
    "content": "ad-variant-a",
    "term": "conveyancing"
  },
  "contact": {
    "firstName": "John",
    "lastName": "Smith",
    "email": "john.smith@example.com",
    "phone": "07700900123",
    "preferredContactMethod": "email | phone | sms"
  },
  "property": {
    "address": "123 High Street",
    "postcode": "SW1A 1AA",
    "propertyType": "detached | semi-detached | terraced | flat | other",
    "estimatedValue": 350000
  },
  "serviceRequest": {
    "serviceType": "purchase | sale | remortgage | transfer | other",
    "timeline": "immediate | 1-month | 3-months | 6-months | exploring",
    "message": "Looking to purchase a property..."
  },
  "gdprConsent": true,
  "marketingConsent": false,
  "referrer": "https://google.com/search",
  "sessionData": {
    "pagesVisited": ["/", "/services", "/contact"],
    "timeOnSite": 120
  }
}
```

**Response:** `201 Created`
```json
{
  "leadId": "463d93be-9e4a-4c87-933b-09627ae723f2",
  "status": "captured | duplicate-merged",
  "capturedAt": "2025-12-12T11:23:00.000Z"
}
```

**Validation Rules:**
- `source` must be one of: contact-form, quote-request, landing-page
- `pageUrl` must be a valid URL
- `contact.firstName` and `contact.lastName` required (2-100 chars)
- `contact.email` must be valid email format (max 254 chars)
- `contact.phone` optional, UK format validation if provided
- `gdprConsent` must be true
- `serviceRequest.serviceType` required

---

#### POST /api/v1/leads/ad-platform-webhook
Receive lead data from ad platform webhooks (Google Ads, Facebook, LinkedIn).

**Request Body:**
```json
{
  "platform": "google-ads | facebook | linkedin | microsoft-ads",
  "campaignId": "12345678",
  "campaignName": "Spring Campaign 2025",
  "adGroupId": "987654321",
  "adId": "ad-001",
  "formId": "form-123",
  "submittedAt": "2025-12-12T10:30:00Z",
  "contact": {
    "firstName": "Jane",
    "lastName": "Doe",
    "email": "jane.doe@example.com",
    "phone": "07700900456"
  },
  "customFields": {
    "interest": "residential-conveyancing",
    "budget": "200000-300000"
  },
  "platformLeadId": "platform-lead-12345"
}
```

**Response:** `200 OK`
```json
{
  "leadId": "563e94cf-0b5b-5d98-a44c-10738bf834f3",
  "status": "captured | duplicate-merged",
  "capturedAt": "2025-12-12T11:23:00.000Z"
}
```

---

#### POST /api/v1/leads/partner-referral
Capture leads from referral partners.

**Request Body:**
```json
{
  "partnerId": "PARTNER001",
  "partnerName": "ABC Introducers Ltd",
  "referralType": "introducer | developer | broker | other",
  "contact": {
    "firstName": "Bob",
    "lastName": "Builder",
    "email": "bob.builder@example.com",
    "phone": "07700900789"
  },
  "property": {
    "address": "456 Park Lane",
    "postcode": "W1K 1AA",
    "propertyType": "flat",
    "estimatedValue": 500000
  },
  "serviceRequest": {
    "serviceType": "purchase",
    "timeline": "1-month",
    "notes": "Urgent purchase for overseas buyer"
  },
  "referralAgreement": {
    "commissionRate": 2.5,
    "commissionType": "percentage | fixed"
  }
}
```

**Response:** `201 Created`
```json
{
  "leadId": "d9ac60ad-129d-4151-9123-b0b5f4081b19",
  "status": "captured | duplicate-merged",
  "capturedAt": "2025-12-12T11:23:00.000Z"
}
```

---

#### POST /api/v1/leads/developer-bulk
Bulk upload leads from property developers (max 1000 leads per request).

**Request Body:**
```json
{
  "developerId": "DEV001",
  "developmentName": "Riverside Gardens",
  "developmentLocation": "London, E1",
  "leads": [
    {
      "contact": {
        "firstName": "Alice",
        "lastName": "Anderson",
        "email": "alice@example.com",
        "phone": "07700900111"
      },
      "property": {
        "developmentUnit": "Block A, Unit 12",
        "plotNumber": "A-12",
        "propertyType": "flat",
        "price": 425000,
        "purchaseStage": "reserved | exchanged | completed"
      },
      "serviceRequest": {
        "serviceType": "purchase",
        "timeline": "immediate"
      }
    }
  ]
}
```

**Response:** `200 OK`
```json
{
  "processed": 1,
  "successful": 1,
  "failed": 0,
  "leadIds": [
    "773f05be-2c6e-5e09-b55d-21849cg945e4"
  ],
  "errors": null
}
```

---

#### POST /api/v1/leads/event-registration
Capture leads from event registrations (webinars, seminars, exhibitions).

**Request Body:**
```json
{
  "eventType": "webinar | seminar | exhibition | workshop",
  "eventName": "First Time Buyer Webinar",
  "eventDate": "2025-12-15T18:00:00Z",
  "registrants": [
    {
      "contact": {
        "firstName": "Charlie",
        "lastName": "Chapman",
        "email": "charlie@example.com",
        "phone": "07700900222"
      },
      "company": "Charlie Corp",
      "jobTitle": "Property Manager",
      "interests": ["buy-to-let", "residential"],
      "questions": ["What are the fees?", "Timeline for completion?"]
    }
  ]
}
```

**Response:** `200 OK`
```json
{
  "registered": 1,
  "leadIds": [
    "884g16cf-3d7f-6f10-c66e-32960dh056f5"
  ]
}
```

---

### Lead Retrieval Endpoints

#### GET /api/v1/leads/{id}
Retrieve a single lead by ID.

**Path Parameters:**
- `id` (required) - GUID of the lead

**Response:** `200 OK`
```json
{
  "id": "b75d4ef3-ca45-4a8d-91f3-29b97dfe05c2",
  "externalLeadId": "76093685-243d-4c53-af66-0210b3f18329",
  "firstName": "Jane",
  "lastName": "Doe",
  "email": "jane.doe@example.com",
  "phone": "+447700900456",
  "address": null,
  "postcode": "SW1A 1AA",
  "propertyType": "flat",
  "serviceType": "purchase",
  "timeline": "3-months",
  "message": null,
  "gdprConsent": true,
  "marketingConsent": true,
  "preferredContactMethod": null,
  "createdAt": "2025-12-12T11:23:23.347Z",
  "updatedAt": "2025-12-12T11:23:23.347Z",
  "score": null,
  "sourceAttributions": [
    {
      "channel": "web-form",
      "source": "contact-form",
      "campaign": null,
      "medium": null,
      "utmSource": null,
      "utmMedium": null,
      "utmCampaign": null,
      "utmContent": null,
      "utmTerm": null,
      "referrer": null,
      "landingPage": "https://example.com/contact",
      "capturedAt": "2025-12-12T11:23:23.346Z"
    }
  ]
}
```

**Error Response:** `404 Not Found`
```json
{
  "error": "Lead not found"
}
```

---

#### GET /api/v1/leads
List leads with filtering and pagination.

**Query Parameters:**
- `page` (optional, default: 1) - Page number for pagination
- `pageSize` (optional, default: 30, max: 100) - Number of results per page
- `source` (optional) - Filter by source (e.g., "PARTNER001", "contact-form")
- `channel` (optional) - Filter by channel (e.g., "web-form", "partner-referral")
- `fromDate` (optional) - Filter leads created after this date (ISO 8601)
- `toDate` (optional) - Filter leads created before this date (ISO 8601)

**Example Request:**
```
GET /api/v1/leads?page=1&pageSize=10&channel=web-form&fromDate=2025-12-01T00:00:00Z
```

**Response:** `200 OK`
```json
{
  "leads": [
    {
      "id": "b75d4ef3-ca45-4a8d-91f3-29b97dfe05c2",
      "firstName": "Jane",
      "lastName": "Doe",
      "email": "jane.doe@example.com",
      ...
    }
  ],
  "totalCount": 42,
  "page": 1,
  "pageSize": 10
}
```

---

## Data Enrichment

The API automatically enriches captured leads:

1. **Phone Number Normalization**: UK phone numbers are converted to E.164 format (+44)
   - Input: `07700900123` → Output: `+447700900123`
   - Input: `+447700900123` → Output: `+447700900123`

2. **Email Normalization**: Emails are converted to lowercase
   - Input: `John.Smith@Example.COM` → Output: `john.smith@example.com`

3. **Postcode Normalization**: UK postcodes are converted to uppercase
   - Input: `sw1a 1aa` → Output: `SW1A 1AA`

4. **Source Attribution**: All lead captures include metadata:
   - Channel (web-form, ad-platform, partner-referral, etc.)
   - Campaign information
   - UTM parameters
   - IP address and User-Agent (for web forms)
   - Timestamp

## Duplicate Detection

The API checks for duplicate leads within a 30-day window:
- Matches on: Email + Phone (if both provided) or Email only
- If duplicate found: Returns status `"duplicate-merged"` instead of `"captured"`
- Existing lead record is updated with new submission timestamp

## Error Responses

### 400 Bad Request
Invalid request data with field-specific errors.

```json
{
  "errors": [
    {
      "propertyName": "Contact.Email",
      "errorMessage": "Valid email address is required",
      "attemptedValue": "invalid-email"
    }
  ]
}
```

### 404 Not Found
Resource not found.

```json
{
  "error": "Lead not found"
}
```

### 500 Internal Server Error
Server error with request ID for tracking.

```json
{
  "error": "An error occurred processing your request",
  "requestId": "0HN7JKQM1234567890"
}
```

## Rate Limiting

Rate limits per configuration:
- **Per API Key**: 1000 requests/hour with burst of 10 requests/second
- **Per IP**: 100 requests/hour with burst of 10 requests/second

When rate limit exceeded:
```json
{
  "error": "Rate limit exceeded",
  "retryAfter": 3600
}
```

## Testing with cURL

### Create a Web Form Lead
```bash
curl -X POST https://localhost:7195/api/v1/leads/web-form \
  -H "Content-Type: application/json" \
  -d '{
    "source": "contact-form",
    "pageUrl": "https://example.com/contact",
    "contact": {
      "firstName": "John",
      "lastName": "Smith",
      "email": "john.smith@example.com",
      "phone": "07700900123"
    },
    "serviceRequest": {
      "serviceType": "purchase"
    },
    "gdprConsent": true
  }'
```

### Get a Lead
```bash
curl https://localhost:7195/api/v1/leads/{guid}
```

### List All Leads
```bash
curl "https://localhost:7195/api/v1/leads?page=1&pageSize=10"
```

## Swagger UI

Interactive API documentation is available at:
- `https://localhost:7195/swagger`

This provides a web interface to explore and test all endpoints.
