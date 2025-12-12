using MarketingAgent.Core.DTOs;
using MarketingAgent.Core.Entities;
using MarketingAgent.Core.Interfaces;

namespace MarketingAgent.Infrastructure.Services;

public class LeadCaptureService : ILeadCaptureService
{
    private readonly ILeadRepository _leadRepository;

    public LeadCaptureService(ILeadRepository leadRepository)
    {
        _leadRepository = leadRepository;
    }

    public async Task<LeadCaptureResponse> CaptureWebFormLeadAsync(
        WebFormLeadRequest request,
        string ipAddress,
        string userAgent,
        CancellationToken cancellationToken = default)
    {
        var isDuplicate = await _leadRepository.ExistsAsync(
            request.Contact.Email,
            request.Contact.Phone,
            DateTime.UtcNow.AddDays(-30),
            cancellationToken);

        var status = isDuplicate ? "duplicate-merged" : "captured";

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            ExternalLeadId = Guid.NewGuid().ToString(),
            FirstName = request.Contact.FirstName,
            LastName = request.Contact.LastName,
            Email = request.Contact.Email.ToLowerInvariant(),
            Phone = NormalizePhoneNumber(request.Contact.Phone),
            Address = request.Property?.Address,
            Postcode = request.Property?.Postcode?.ToUpperInvariant(),
            PropertyType = request.Property?.PropertyType,
            ServiceType = request.ServiceRequest.ServiceType,
            Timeline = request.ServiceRequest.Timeline,
            Message = request.ServiceRequest.Message,
            GdprConsent = request.GdprConsent,
            MarketingConsent = request.MarketingConsent ?? false,
            PreferredContactMethod = request.Contact.PreferredContactMethod
        };

        lead.SourceAttributions.Add(new LeadSourceAttribution
        {
            Id = Guid.NewGuid(),
            LeadId = lead.Id,
            Channel = "web-form",
            Source = request.Source,
            UtmSource = request.UtmParams?.Source,
            UtmMedium = request.UtmParams?.Medium,
            UtmCampaign = request.UtmParams?.Campaign,
            UtmContent = request.UtmParams?.Content,
            UtmTerm = request.UtmParams?.Term,
            Referrer = request.Referrer,
            LandingPage = request.PageUrl,
            IpAddress = ipAddress,
            UserAgent = userAgent
        });

        await _leadRepository.CreateAsync(lead, cancellationToken);

        return new LeadCaptureResponse(lead.Id, status, lead.CreatedAt);
    }

    public async Task<LeadCaptureResponse> CaptureAdPlatformLeadAsync(
        AdPlatformWebhookRequest request,
        CancellationToken cancellationToken = default)
    {
        var isDuplicate = await _leadRepository.ExistsAsync(
            request.Contact.Email,
            request.Contact.Phone,
            DateTime.UtcNow.AddDays(-30),
            cancellationToken);

        var status = isDuplicate ? "duplicate-merged" : "captured";

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            ExternalLeadId = request.PlatformLeadId,
            FirstName = request.Contact.FirstName,
            LastName = request.Contact.LastName,
            Email = request.Contact.Email.ToLowerInvariant(),
            Phone = NormalizePhoneNumber(request.Contact.Phone),
            ServiceType = "other",
            GdprConsent = true,
            MarketingConsent = false
        };

        lead.SourceAttributions.Add(new LeadSourceAttribution
        {
            Id = Guid.NewGuid(),
            LeadId = lead.Id,
            Channel = "ad-platform",
            Source = request.Platform,
            Campaign = request.CampaignName,
            Medium = "paid"
        });

        await _leadRepository.CreateAsync(lead, cancellationToken);

        return new LeadCaptureResponse(lead.Id, status, lead.CreatedAt);
    }

    public async Task<LeadCaptureResponse> CapturePartnerReferralAsync(
        PartnerReferralRequest request,
        CancellationToken cancellationToken = default)
    {
        var isDuplicate = await _leadRepository.ExistsAsync(
            request.Contact.Email,
            request.Contact.Phone,
            DateTime.UtcNow.AddDays(-30),
            cancellationToken);

        var status = isDuplicate ? "duplicate-merged" : "captured";

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            ExternalLeadId = Guid.NewGuid().ToString(),
            FirstName = request.Contact.FirstName,
            LastName = request.Contact.LastName,
            Email = request.Contact.Email.ToLowerInvariant(),
            Phone = NormalizePhoneNumber(request.Contact.Phone),
            Address = request.Property?.Address,
            Postcode = request.Property?.Postcode?.ToUpperInvariant(),
            PropertyType = request.Property?.PropertyType,
            ServiceType = request.ServiceRequest.ServiceType,
            Timeline = request.ServiceRequest.Timeline,
            Message = request.ServiceRequest.Message,
            GdprConsent = true,
            MarketingConsent = false,
            PreferredContactMethod = request.Contact.PreferredContactMethod
        };

        lead.SourceAttributions.Add(new LeadSourceAttribution
        {
            Id = Guid.NewGuid(),
            LeadId = lead.Id,
            Channel = "partner-referral",
            Source = request.PartnerId,
            Campaign = request.PartnerName,
            Medium = request.ReferralType
        });

        await _leadRepository.CreateAsync(lead, cancellationToken);

        return new LeadCaptureResponse(lead.Id, status, lead.CreatedAt);
    }

    public async Task<BulkLeadCaptureResponse> CaptureDeveloperBulkLeadsAsync(
        DeveloperBulkRequest request,
        CancellationToken cancellationToken = default)
    {
        var leadIds = new List<Guid>();
        var errors = new List<BulkLeadError>();
        var successful = 0;

        for (int i = 0; i < request.Leads.Count && i < 1000; i++)
        {
            try
            {
                var developerLead = request.Leads[i];
                var lead = new Lead
                {
                    Id = Guid.NewGuid(),
                    ExternalLeadId = Guid.NewGuid().ToString(),
                    FirstName = developerLead.Contact.FirstName,
                    LastName = developerLead.Contact.LastName,
                    Email = developerLead.Contact.Email.ToLowerInvariant(),
                    Phone = NormalizePhoneNumber(developerLead.Contact.Phone),
                    PropertyType = developerLead.Property.PropertyType,
                    ServiceType = developerLead.ServiceRequest.ServiceType,
                    Timeline = developerLead.ServiceRequest.Timeline,
                    Message = developerLead.ServiceRequest.Message,
                    GdprConsent = true,
                    MarketingConsent = false
                };

                lead.SourceAttributions.Add(new LeadSourceAttribution
                {
                    Id = Guid.NewGuid(),
                    LeadId = lead.Id,
                    Channel = "developer-bulk",
                    Source = request.DeveloperId,
                    Campaign = request.DevelopmentName,
                    Medium = "bulk-upload"
                });

                await _leadRepository.CreateAsync(lead, cancellationToken);
                leadIds.Add(lead.Id);
                successful++;
            }
            catch (Exception ex)
            {
                errors.Add(new BulkLeadError(i, ex.Message));
            }
        }

        return new BulkLeadCaptureResponse(
            request.Leads.Count,
            successful,
            errors.Count,
            leadIds,
            errors.Any() ? errors : null);
    }

    public async Task<EventRegistrationResponse> CaptureEventRegistrationAsync(
        EventRegistrationRequest request,
        CancellationToken cancellationToken = default)
    {
        var leadIds = new List<Guid>();

        foreach (var registrant in request.Registrants)
        {
            var lead = new Lead
            {
                Id = Guid.NewGuid(),
                ExternalLeadId = Guid.NewGuid().ToString(),
                FirstName = registrant.Contact.FirstName,
                LastName = registrant.Contact.LastName,
                Email = registrant.Contact.Email.ToLowerInvariant(),
                Phone = NormalizePhoneNumber(registrant.Contact.Phone),
                ServiceType = "other",
                GdprConsent = true,
                MarketingConsent = false,
                PreferredContactMethod = registrant.Contact.PreferredContactMethod
            };

            lead.SourceAttributions.Add(new LeadSourceAttribution
            {
                Id = Guid.NewGuid(),
                LeadId = lead.Id,
                Channel = "event-registration",
                Source = request.EventType,
                Campaign = request.EventName,
                Medium = "event"
            });

            await _leadRepository.CreateAsync(lead, cancellationToken);
            leadIds.Add(lead.Id);
        }

        return new EventRegistrationResponse(leadIds.Count, leadIds);
    }

    public async Task<LeadResponse?> GetLeadByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lead = await _leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead == null) return null;

        return MapToLeadResponse(lead);
    }

    public async Task<LeadListResponse> GetLeadsAsync(
        int page,
        int pageSize,
        string? source,
        string? channel,
        DateTime? fromDate,
        DateTime? toDate,
        CancellationToken cancellationToken = default)
    {
        var (leads, totalCount) = await _leadRepository.GetAllAsync(
            page,
            pageSize,
            source,
            channel,
            fromDate,
            toDate,
            cancellationToken);

        var leadResponses = leads.Select(MapToLeadResponse).ToList();

        return new LeadListResponse(leadResponses, totalCount, page, pageSize);
    }

    private static string? NormalizePhoneNumber(string? phone)
    {
        if (string.IsNullOrEmpty(phone)) return null;

        var digits = new string(phone.Where(char.IsDigit).ToArray());

        if (digits.StartsWith("0") && digits.Length == 11)
        {
            return "+44" + digits[1..];
        }

        if (digits.StartsWith("44") && digits.Length == 12)
        {
            return "+" + digits;
        }

        return phone;
    }

    private static LeadResponse MapToLeadResponse(Lead lead)
    {
        return new LeadResponse(
            lead.Id,
            lead.ExternalLeadId,
            lead.FirstName,
            lead.LastName,
            lead.Email,
            lead.Phone,
            lead.Address,
            lead.Postcode,
            lead.PropertyType,
            lead.ServiceType,
            lead.Timeline,
            lead.Message,
            lead.GdprConsent,
            lead.MarketingConsent,
            lead.PreferredContactMethod,
            lead.CreatedAt,
            lead.UpdatedAt,
            lead.LeadScore != null ? new LeadScoreResponse(
                lead.LeadScore.OverallScore,
                lead.LeadScore.Tier,
                lead.LeadScore.CompletenessScore,
                lead.LeadScore.EngagementScore,
                lead.LeadScore.ReadinessScore,
                lead.LeadScore.SourceQualityScore,
                lead.LeadScore.CalculatedAt) : null,
            lead.SourceAttributions.Select(a => new LeadSourceAttributionResponse(
                a.Channel,
                a.Source,
                a.Campaign,
                a.Medium,
                a.UtmSource,
                a.UtmMedium,
                a.UtmCampaign,
                a.UtmContent,
                a.UtmTerm,
                a.Referrer,
                a.LandingPage,
                a.CapturedAt)).ToList()
        );
    }
}
