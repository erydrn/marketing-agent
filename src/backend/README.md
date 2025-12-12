# Marketing Agent API - Backend Implementation

## Overview
This is the backend API implementation for the Marketing Agent Digital Lead Capture system. It provides RESTful endpoints for capturing leads from multiple marketing channels.

## Architecture

### Project Structure
- **MarketingAgent.Api** - ASP.NET Core Web API with Minimal APIs
- **MarketingAgent.Core** - Domain models, interfaces, DTOs, and validators
- **MarketingAgent.Infrastructure** - Data access, repositories, and services
- **MarketingAgent.Tests** - Unit and integration tests

### Technology Stack
- .NET 10.0
- ASP.NET Core Minimal APIs
- Entity Framework Core 10
- FluentValidation
- Swashbuckle (Swagger/OpenAPI)
- xUnit, Moq, FluentAssertions (Testing)

## API Endpoints

### Lead Capture Endpoints

1. **POST /api/v1/leads/web-form** - Capture from website forms
2. **POST /api/v1/leads/ad-platform-webhook** - Capture from ad platforms (Google Ads, Facebook, LinkedIn)
3. **POST /api/v1/leads/partner-referral** - Capture partner referrals
4. **POST /api/v1/leads/developer-bulk** - Bulk upload from developers (max 1000 leads)
5. **POST /api/v1/leads/event-registration** - Event registrations (webinars, seminars)

### Lead Retrieval Endpoints

6. **GET /api/v1/leads/{id}** - Get single lead by ID
7. **GET /api/v1/leads** - List leads with filtering and pagination

### Health Check

- **GET /health** - Basic health check

## Running the Application

### Prerequisites
- .NET 10 SDK or later
- Your favorite IDE (VS Code, Visual Studio, Rider)

### Build
```bash
dotnet build MarketingAgent.sln
```

### Run
```bash
cd src/backend/MarketingAgent.Api
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

### Run Tests
```bash
dotnet test MarketingAgent.sln
```

## Configuration

The application uses an in-memory database by default for development. To use SQL Server, update `Program.cs`:

```csharp
builder.Services.AddDbContext<MarketingAgentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

And add to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MarketingAgent;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## Features Implemented

### Data Validation
✅ Required field validation  
✅ Email format validation  
✅ UK phone number format validation  
✅ UK postcode format validation  
✅ Service type enumeration validation  
✅ Message length limits (5000 chars)  
✅ GDPR consent validation  

### Data Enrichment
✅ Phone number normalization to +44 format  
✅ Email normalization (lowercase)  
✅ Postcode normalization (uppercase)  
✅ Source attribution capture  
✅ Timestamp capture  
✅ IP address and user agent capture  

### Duplicate Detection
✅ Check email + phone within 30-day window  
✅ Flag as duplicate (status: "duplicate-merged")  
✅ Update existing lead metadata  

### Error Handling
✅ 400 Bad Request - Invalid data with field-specific errors  
✅ 404 Not Found - Lead not found  
✅ 500 Internal Server Error - Server issues  

## Example Requests

### Web Form Lead Capture
```bash
curl -X POST https://localhost:5001/api/v1/leads/web-form \
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

### Get Lead by ID
```bash
curl https://localhost:5001/api/v1/leads/{guid}
```

### List Leads with Filtering
```bash
curl "https://localhost:5001/api/v1/leads?page=1&pageSize=10&channel=web-form"
```

## Next Steps

- [ ] Add API key authentication
- [ ] Implement rate limiting
- [ ] Add webhook signature verification
- [ ] Implement postcode lookup service integration
- [ ] Add comprehensive integration tests
- [ ] Add performance tests
- [ ] Create database migrations for production
- [ ] Add Application Insights telemetry
- [ ] Implement circuit breaker for external services

## License
MIT
