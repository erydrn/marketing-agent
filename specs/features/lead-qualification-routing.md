# Feature Requirements Document: Lead Qualification & Routing

**Feature ID:** FRD-002  
**Feature Name:** Lead Qualification & Routing  
**Priority:** P0 (Critical - Foundation)  
**Status:** Draft  
**Last Updated:** December 12, 2025

---

## 1. Feature Overview

### Purpose
Automatically assess lead quality, prioritize leads based on conversion potential, and intelligently route qualified leads to the Sales Agent with appropriate context and urgency flags.

### Business Value
- Reduce Sales Agent workload by filtering out unqualified leads
- Prioritize high-value opportunities for faster response
- Improve conversion rates through intelligent lead scoring
- Ensure leads reach the right team with the right information
- Reduce time from lead capture to meaningful engagement

### Links to PRD
- Supports PRD [REQ-2]: Automatic routing to Sales Agent with context
- Supports PRD [REQ-4]: Data quality validation before handoff
- Supports PRD [REQ-7]: Seamless integration with Sales and New Business Agents
- Supports PRD [REQ-8]: Lead nurturing for not-ready prospects

### Feature Inputs
- **Captured Lead Data:** Output from FRD-001 (all lead fields, source attribution)
- **Scoring Configuration:** Weights for completeness, engagement, readiness, source quality
- **Qualification Rules:** Geographic boundaries, service type matching, exclusion lists
- **Business Hours Settings:** Operating hours, SLA definitions, escalation policies
- **Historical Performance Data:** Previous lead conversion rates by source/score
- **Sales Agent Capacity:** Available reps, queue depth, current load

### Feature Outputs
- **Lead Score (0-100):** Calculated quality score with tier assignment (Hot/Warm/Cool/Cold)
- **Qualification Decision:** Qualified, disqualified (with reason), or requires manual review
- **Routing Destination:** Sales Agent, New Business Agent, Nurture Queue, or Holding Queue
- **Priority Flag:** Critical, High, Standard, or Low with SLA deadline
- **Enriched Context Package:** Recommended actions, talking points, potential objections
- **Sales Agent Handoff:** Complete data package via FRD-004 integration
- **Disqualification Reports:** Reasons, patterns, quality improvement insights
- **Queue Metrics:** Depth, average wait time, processing speed

### Technical Constraints
- Scoring must complete within 2 seconds to maintain real-time user experience
- Enrichment services (property lookup, address validation) may introduce latency (up to 8 additional seconds)
- Geographic qualification requires accurate postcode database (UK-specific)
- Duplicate detection limited to 30-day lookback window for performance
- Queue management requires persistent storage to survive system restarts
- Scoring algorithm changes affect all leads uniformly (cannot A/B test scoring in production easily)
- Re-engagement detection accuracy depends on email/phone matching (variations cause misses)

---

## 2. User Personas

### Primary Users
- **Sales Agent (System):** Receives qualified leads with priority rankings
- **Sales Team Member:** Acts on leads routed through Sales Agent
- **Marketing Manager:** Monitors qualification criteria effectiveness

### Secondary Users
- **New Business Agent (System):** Receives leads requiring manual review
- **Business Development Manager:** Reviews partner/developer lead quality

---

## 3. Functional Requirements

### 3.1 Lead Scoring Engine

**Capability:** Assign quality scores to incoming leads based on multiple factors

#### Scoring Dimensions

**Completeness Score (0-25 points):**
- All required fields present: +25
- Missing 1 optional field: +20
- Missing 2+ optional fields: +15
- Missing required fields: +0 (flagged for manual review)

**Engagement Score (0-25 points):**
- Direct contact attempt (form submission): +25
- Content download or event registration: +15
- Newsletter signup only: +10
- No direct engagement: +5

**Readiness Score (0-25 points):**
- Indicated timeline: "Immediate" or "Within 1 month": +25
- Timeline: "1-3 months": +20
- Timeline: "3-6 months": +15
- Timeline: "Just exploring": +10
- No timeline provided: +5

**Source Quality Score (0-25 points):**
- Referral from existing partner: +25
- Developer lead with property details: +25
- Paid search (high-intent keywords): +20
- Organic search: +15
- Social media: +10
- Unknown source: +5

**Total Lead Score:** 0-100 points

#### Lead Tiers
- **Hot Lead (80-100):** Immediate Sales Agent routing, highest priority
- **Warm Lead (60-79):** Sales Agent routing, standard priority
- **Cool Lead (40-59):** Nurture campaign, delayed Sales Agent routing
- **Cold Lead (0-39):** Nurture campaign only, manual review queue

**Acceptance Criteria:**
- Score calculated within 2 seconds of lead capture
- Score stored with lead record for reporting
- Score recalculated if lead data is updated
- Scoring rules configurable by Marketing Manager

### 3.2 Lead Qualification Rules

**Capability:** Apply business logic to determine if lead meets minimum qualification standards

#### Automatic Qualification Criteria

**Must Meet ALL of:**
- Valid UK postcode or address
- Valid email or phone number
- Lead type matches service offering (e.g., residential conveyancing)
- Not on exclusion list (competitors, spam domains)

**Geographic Qualification:**
- Property location within serviceable areas
- Flagged if outside primary coverage (not rejected)
- Special handling for high-value areas

**Service Type Matching:**
- Purchase/Sale/Remortgage clearly indicated
- Property type matches service capability
- Transaction value within acceptable range (if provided)

**Acceptance Criteria:**
- Qualification decision made within 3 seconds
- Disqualification reasons logged for analysis
- Disqualified leads sent to holding queue for review
- Edge cases flagged for manual review rather than auto-rejection

### 3.3 Intelligent Routing

**Capability:** Route qualified leads to appropriate destination based on lead characteristics

#### Routing Destinations

**1. Sales Agent (Primary Path):**
- All "Hot" and "Warm" leads
- Complete lead data package includes:
  - Lead score and tier
  - Source attribution
  - Next recommended action
  - Priority flag (urgent vs. standard)
  - Best contact method and time

**2. New Business Agent (Manual Review):**
- Incomplete data (missing required fields)
- Suspicious patterns (potential spam)
- High-value but complex scenarios
- Partner referrals requiring special handling
- Data validation failures

**3. Nurture Campaign Queue:**
- "Cool" and "Cold" leads
- Leads with distant timelines
- Leads requesting information only
- Newsletter subscribers without purchase intent

**4. Holding Queue (Temporary):**
- Duplicate submissions pending merge decision
- Leads during system maintenance windows
- Batch uploads pending validation

#### Routing Logic Priority

1. **Urgency Flags:** Immediate need, hot transfer from call
2. **Lead Tier:** Hot > Warm > Cool > Cold
3. **Source Type:** Referral partner > Developer > Paid > Organic
4. **Time in Queue:** First in, first out within same tier
5. **Business Hours:** Route to on-call during off-hours vs. queue for morning

**Acceptance Criteria:**
- Routing decision made within 5 seconds of qualification
- Sales Agent receives leads in priority order
- Lead status updated to "Routed" with timestamp
- Routing failures trigger fallback to holding queue
- Manual override capability for priority leads

### 3.4 Lead Enrichment & Context Building

**Capability:** Add contextual information to help Sales Agent personalize outreach

#### Data Enrichment

**Property Information:**
- Address lookup from postcode
- Estimated property value (from public records if available)
- Local market conditions (average prices, sale times)
- Property type and characteristics

**Lead Source Context:**
- Campaign name and messaging used
- Ad creative or landing page that converted
- UTM parameters and keywords
- Referring partner details and relationship

**Behavioral Context:**
- Pages visited on website before conversion
- Content downloaded or events attended
- Email open and click history (if applicable)
- Previous interactions with company

**Recommended Next Actions:**
- Suggested contact script based on source
- Key talking points from their inquiry
- Recommended response timeframe
- Potential objections or concerns to address

**Acceptance Criteria:**
- Enrichment completes within 10 seconds
- Missing enrichment data doesn't block routing
- Sources of enriched data are documented
- Privacy compliance maintained for all lookups

### 3.5 Priority & Urgency Management

**Capability:** Flag high-priority leads requiring immediate attention

#### Priority Flags

**Critical Priority (Immediate Action):**
- Lead score 90-100
- Referral from VIP partner
- High-value transaction (if indicated)
- Hot transfer from phone call
- Competitive situation indicated

**High Priority (Same Day Response):**
- Lead score 80-89
- Developer lead with imminent purchase
- Repeat inquiry from previous lead
- Time-sensitive inquiry (completion deadline mentioned)

**Standard Priority (24-48 Hour Response):**
- Lead score 60-79
- Standard inquiries
- General information requests with purchase intent

**Low Priority (Nurture Campaign):**
- Lead score < 60
- Information gathering only
- Distant timeline
- Newsletter signups

**Acceptance Criteria:**
- Priority visible to Sales Agent immediately
- Email/SMS alerts sent for Critical priority
- SLA timers start based on priority level
- Priority can be manually adjusted by Sales team

---

## 4. Lead Handoff Specifications

### 4.1 Sales Agent Integration

**Data Package Format:**
```
Lead ID: Unique identifier
Score: 0-100
Tier: Hot/Warm/Cool/Cold
Priority: Critical/High/Standard/Low
Source Channel: Digital Ads/Website/Partner/Developer/PR/Organic
Source Detail: Campaign name, partner name, etc.
Contact Information: Name, email, phone, preferred contact method
Property Details: Address, type, estimated value
Service Request: Purchase/Sale/Remortgage/Other
Timeline: Immediate/1 month/3 months/6+ months/Unknown
Lead Context: Behavioral history, content engaged with
Recommended Actions: Next steps, talking points
Routing Timestamp: When sent to Sales Agent
SLA Deadline: When follow-up must occur by
```

**Delivery Method:**
- Real-time API call to Sales Agent system
- Webhook notification for critical priority leads
- Batch processing for low-priority leads (hourly)

**Acceptance Criteria:**
- 100% of qualified leads successfully handed off
- Handoff confirmed with acknowledgment from Sales Agent
- Failed handoffs retry 3 times before alerting
- Data integrity validated before and after handoff

### 4.2 New Business Agent Integration

**Scenarios Requiring Manual Review:**
- Data validation failures
- Suspected spam or duplicate
- VIP client requiring white-glove service
- Complex scenarios outside standard workflows
- Partner leads with special commission structures

**Data Package:**
- All captured lead data
- Reason for manual review flag
- Suggested next steps
- Original source information preserved

**Acceptance Criteria:**
- Manual review queue visible to New Business team
- Queue prioritized by urgency
- Team can accept, reject, or edit and re-route lead
- Decisions logged for quality improvement

---

## 5. Business Rules & Logic

### 5.1 Duplicate Handling

**Detection:**
- Exact match on email + phone = duplicate
- Same name + same property address = likely duplicate
- Same IP + multiple submissions within 1 hour = potential spam

**Resolution:**
- Merge duplicate records, keeping most recent submission
- Append new information to existing lead
- Increase lead score if re-engagement detected
- Notify Sales Agent of additional touchpoint

### 5.2 Re-engagement Logic

**Scenario:** Previous lead returns after no conversion

**Rules:**
- Increase lead score by 10 points (re-engagement signal)
- Flag to Sales Agent as "Returning Lead"
- Include previous interaction history
- Route to same Sales rep if available

### 5.3 Time-Based Rules

**Business Hours Routing:**
- Mon-Fri 9am-5pm: Immediate routing to Sales Agent
- Evenings/Weekends: Queue for next business day unless Critical priority
- Critical priority leads: Alert on-call team regardless of time

**Aging Rules:**
- Leads not contacted within SLA: Escalation alert
- Leads in queue > 48 hours: Review for re-scoring
- Stale leads (30+ days): Move to long-term nurture

---

## 6. Performance Requirements

### Processing Speed
- Lead scoring: < 2 seconds
- Qualification decision: < 3 seconds
- Enrichment: < 10 seconds
- Routing: < 5 seconds
- End-to-end (capture to Sales Agent): < 20 seconds

### Accuracy
- Scoring consistency: 100% (same inputs = same score)
- Qualification accuracy: > 98% (manual review validates)
- Routing accuracy: 100% (leads reach intended destination)

### Throughput
- Process 100 leads concurrently
- Handle 1,000 leads per hour
- Scale to 5,000 leads per hour during campaigns

---

## 7. Monitoring & Alerting

### Real-Time Alerts

**Trigger Alerts For:**
- Critical priority lead captured (immediate SMS/email to Sales)
- Routing failure after 3 retries
- Qualification error rate > 5% in 1 hour
- Lead scoring service unavailable
- Queue depth exceeds threshold (50+ leads waiting)

**Alert Recipients:**
- Sales Manager: Critical priority leads, SLA breaches
- Marketing Manager: Qualification errors, scoring issues
- Technical Team: System failures, integration errors

### Dashboard Metrics

**Real-Time View:**
- Leads in queue by tier
- Average time in queue
- Routing success rate
- Current processing speed

**Daily Summary:**
- Leads qualified vs. disqualified
- Score distribution
- Routing destinations breakdown
- SLA compliance rate

---

## 8. Success Metrics

### Operational Metrics
- **Qualification Rate:** % of captured leads that qualify (target: > 70%)
- **Average Processing Time:** Capture to Sales Agent (target: < 15 seconds)
- **Routing Accuracy:** % routed to correct destination (target: 100%)
- **SLA Compliance:** % of leads contacted within SLA (target: > 95%)

### Business Metrics
- **Lead-to-Opportunity Conversion:** % of qualified leads becoming opportunities (target: improvement over baseline)
- **Lead Quality Score:** Average score of converted leads vs. non-converted
- **False Positive Rate:** % qualified leads that shouldn't have been (target: < 5%)
- **Sales Team Satisfaction:** NPS score from Sales on lead quality

---

## 9. User Stories

```gherkin
As a Sales Agent system,
I want to receive only qualified leads with complete information,
So that I can immediately engage prospects without requesting basic details.
```

```gherkin
As a Sales team member,
I want to see lead priority clearly indicated,
So that I can focus my time on the highest-value opportunities.
```

```gherkin
As a Marketing Manager,
I want to understand which lead sources produce the highest quality leads,
So that I can optimize budget allocation.
```

```gherkin
As a New Business team member,
I want leads with data quality issues flagged for my review,
So that I can salvage potentially valuable leads that would otherwise be lost.
```

```gherkin
As a potential client with an urgent need,
I want my inquiry to be prioritized and get a rapid response,
So that I can move forward with my transaction quickly.
```

---

## 10. Out of Scope

- Lead nurturing campaign content and sequencing (separate feature)
- Sales Agent's contact strategy and script (separate system)
- CRM functionality beyond routing
- Commission calculation for referral partners
- A/B testing of qualification criteria
- Machine learning-based scoring (future enhancement)

---

## 11. Dependencies

### Feature Dependencies
- **Upstream:** FRD-001 (Lead Capture) must deliver complete lead data before qualification can occur
- **Downstream:** FRD-004 (Sales Agent Integration) receives qualified leads from this feature
- **Downstream:** FRD-003 (Analytics & Reporting) depends on scoring and routing decisions
- **Parallel:** Nurture campaign system (separate feature, out of current scope) receives Cool/Cold leads

### Technical Dependencies

**Data Sources (Critical):**
- Lead database from FRD-001 (read/write access)
- Geographic reference data (UK postcodes, serviceable areas)
- Historical conversion data for scoring model tuning
- Sales Agent API availability (for handoff)

**Processing Services:**
- Property data enrichment service (optional, non-blocking)
- Duplicate detection database (30-day rolling window)
- Lead scoring engine (configurable rules engine)
- Queue management system (persistent storage)

**Integration Points:**
- Sales Agent system endpoint (FRD-004)
- New Business Agent workflow (manual review queue)
- Nurture campaign platform (for Cool/Cold leads)

### Business Dependencies

**Policy & Configuration:**
- Scoring weights approved by Marketing and Sales leadership
- Geographic serviceable areas defined
- Service type matching rules established
- SLA response times agreed upon
- Disqualification policy (reject vs. review)

**Operational:**
- Sales team capacity to handle lead volume
- New Business team availability for manual reviews
- Escalation contacts defined for SLA breaches
- Re-scoring triggers and frequency defined

**Data:**
- Competitor domain list (for exclusions)
- VIP partner list (priority routing)
- Historical baseline conversion rates by source

---

## 12. Implementation Considerations

### Phase 1 (MVP)
- Basic scoring model (completeness + source type only)
- Simple qualification rules (geography + required fields)
- Direct routing to Sales Agent or manual review queue
- Critical priority flagging only

### Phase 2 (Enhanced)
- Full scoring model with all dimensions
- Lead enrichment from external data sources
- Automated nurture campaign routing
- Re-engagement detection

### Phase 3 (Optimized)
- Machine learning-enhanced scoring
- Predictive conversion probability
- Dynamic scoring adjustments based on outcomes
- Advanced behavioral analysis

---

## 13. Configuration & Customization

### Configurable Parameters

**Marketing Manager Can Adjust:**
- Scoring weights for each dimension
- Qualification thresholds
- Lead tier boundaries (e.g., Hot = 85+ vs. 80+)
- Source quality rankings
- Geographic priority areas

**Cannot Be Changed Without Developer:**
- Core routing logic
- Integration endpoints
- Data validation rules
- SLA monitoring logic

### A/B Testing Capability

**Support for Testing:**
- Different scoring models on lead cohorts
- Qualification criteria variations
- Routing strategies
- Measure impact on conversion rates

---

## 14. Open Questions

1. **Scoring Weights:** What relative importance should each scoring dimension have? (Requires Sales team input)
2. **Geographic Limits:** What specific postcodes/regions are serviceable vs. out-of-area?
3. **SLA Standards:** What are the official response time SLAs by lead tier?
4. **Disqualification Policy:** Should we ever fully reject a lead or always queue for review?
5. **VIP Lists:** Are there specific partners or sources that should always get priority regardless of score?
6. **Re-engagement Window:** How long before a returning lead is considered new vs. re-engagement?
7. **Manual Override:** Who has authority to manually change lead priority or routing?
