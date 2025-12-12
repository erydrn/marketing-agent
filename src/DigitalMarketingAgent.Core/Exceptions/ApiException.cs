namespace DigitalMarketingAgent.Core.Exceptions;

/// <summary>
/// Base exception for all API exceptions
/// </summary>
public abstract class ApiException : Exception
{
    /// <summary>
    /// HTTP status code for this exception
    /// </summary>
    public abstract int StatusCode { get; }

    protected ApiException(string message) : base(message)
    {
    }

    protected ApiException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
