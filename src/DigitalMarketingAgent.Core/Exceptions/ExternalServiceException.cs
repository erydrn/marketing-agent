namespace DigitalMarketingAgent.Core.Exceptions;

/// <summary>
/// Exception thrown when an external service call fails
/// </summary>
public class ExternalServiceException : ApiException
{
    public override int StatusCode => 503;

    public string ServiceName { get; }

    public ExternalServiceException(string serviceName, string message) 
        : base($"External service '{serviceName}' error: {message}")
    {
        ServiceName = serviceName;
    }

    public ExternalServiceException(string serviceName, string message, Exception innerException) 
        : base($"External service '{serviceName}' error: {message}", innerException)
    {
        ServiceName = serviceName;
    }
}
