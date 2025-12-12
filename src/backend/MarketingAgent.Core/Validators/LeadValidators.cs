using FluentValidation;
using MarketingAgent.Core.DTOs;

namespace MarketingAgent.Core.Validators;

/// <summary>
/// Validator for web form lead requests
/// </summary>
public class WebFormLeadRequestValidator : AbstractValidator<WebFormLeadRequest>
{
    public WebFormLeadRequestValidator()
    {
        RuleFor(x => x.Source)
            .NotEmpty()
            .WithMessage("Source is required")
            .Must(s => new[] { "contact-form", "quote-request", "landing-page" }.Contains(s))
            .WithMessage("Invalid source type");

        RuleFor(x => x.PageUrl)
            .NotEmpty()
            .WithMessage("Page URL is required")
            .Must(BeValidUrl)
            .WithMessage("Invalid page URL format");

        RuleFor(x => x.Contact)
            .NotNull()
            .SetValidator(new ContactDtoValidator());

        RuleFor(x => x.ServiceRequest)
            .NotNull()
            .SetValidator(new ServiceRequestDtoValidator());

        RuleFor(x => x.GdprConsent)
            .Equal(true)
            .WithMessage("GDPR consent is required");

        When(x => x.Property != null, () =>
        {
            RuleFor(x => x.Property!)
                .SetValidator(new PropertyDtoValidator());
        });
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

/// <summary>
/// Validator for contact information
/// </summary>
public class ContactDtoValidator : AbstractValidator<ContactDto>
{
    public ContactDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 100)
            .WithMessage("First name must be between 2 and 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 100)
            .WithMessage("Last name must be between 2 and 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254)
            .WithMessage("Valid email address is required");

        When(x => !string.IsNullOrEmpty(x.Phone), () =>
        {
            RuleFor(x => x.Phone!)
                .Matches(@"^(\+44|0)[0-9]{10}$")
                .WithMessage("Phone must be a valid UK phone number");
        });

        When(x => !string.IsNullOrEmpty(x.PreferredContactMethod), () =>
        {
            RuleFor(x => x.PreferredContactMethod!)
                .Must(m => new[] { "email", "phone", "sms" }.Contains(m))
                .WithMessage("Preferred contact method must be email, phone, or sms");
        });
    }
}

/// <summary>
/// Validator for property information
/// </summary>
public class PropertyDtoValidator : AbstractValidator<PropertyDto>
{
    public PropertyDtoValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Address), () =>
        {
            RuleFor(x => x.Address!)
                .MaximumLength(500)
                .WithMessage("Address must not exceed 500 characters");
        });

        When(x => !string.IsNullOrEmpty(x.Postcode), () =>
        {
            RuleFor(x => x.Postcode!)
                .Matches(@"^[A-Z]{1,2}[0-9]{1,2}[A-Z]?\s?[0-9][A-Z]{2}$")
                .WithMessage("Postcode must be a valid UK postcode");
        });

        When(x => !string.IsNullOrEmpty(x.PropertyType), () =>
        {
            RuleFor(x => x.PropertyType!)
                .Must(t => new[] { "detached", "semi-detached", "terraced", "flat", "other" }.Contains(t))
                .WithMessage("Invalid property type");
        });
    }
}

/// <summary>
/// Validator for service request information
/// </summary>
public class ServiceRequestDtoValidator : AbstractValidator<ServiceRequestDto>
{
    public ServiceRequestDtoValidator()
    {
        RuleFor(x => x.ServiceType)
            .NotEmpty()
            .Must(t => new[] { "purchase", "sale", "remortgage", "transfer", "other" }.Contains(t))
            .WithMessage("Invalid service type");

        When(x => !string.IsNullOrEmpty(x.Timeline), () =>
        {
            RuleFor(x => x.Timeline!)
                .Must(t => new[] { "immediate", "1-month", "3-months", "6-months", "exploring" }.Contains(t))
                .WithMessage("Invalid timeline");
        });

        When(x => !string.IsNullOrEmpty(x.Message), () =>
        {
            RuleFor(x => x.Message!)
                .MaximumLength(5000)
                .WithMessage("Message must not exceed 5000 characters");
        });
    }
}
