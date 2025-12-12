namespace DigitalMarketingAgent.Core.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found
/// </summary>
public class NotFoundException : ApiException
{
    public override int StatusCode => 404;

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string resourceType, object resourceId) 
        : base($"{resourceType} with identifier '{resourceId}' was not found.")
    {
    }
}
