using System.Net;

namespace Haskap.DugunSalonu.Presentation.WebApi;
public class Envelope<T>
{
    public T? Result { get; }
    public string? ExceptionMessage { get; }
    public string? ExceptionStackTrace { get; protected set; }
    public DateTime TimeGenerated { get; }
    public bool HasError => HttpStatusCode != HttpStatusCode.OK; //!string.IsNullOrWhiteSpace(ExceptionMessage);
    public string? ExceptionType { get; }
    public HttpStatusCode HttpStatusCode { get; }

    public Envelope()
    {
    }

    protected internal Envelope(T? result, string? exceptionMessage, string? exceptionStackTrace, string? exceptionType, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        Result = result;
        ExceptionMessage = exceptionMessage;
        ExceptionStackTrace = exceptionStackTrace;
        ExceptionType = exceptionType;
        TimeGenerated = DateTime.UtcNow;
        HttpStatusCode = httpStatusCode;
    }
}

public sealed class Envelope : Envelope<object>
{
    private Envelope(string? exceptionMessage, string? exceptionStackTrace, string? exceptionType, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        : base(null, exceptionMessage, exceptionStackTrace, exceptionType, httpStatusCode)
    {
    }

    public static Envelope<T> Ok<T>(T result)
    {
        return new Envelope<T>(result, null, null, null);
    }

    public static Envelope Ok()
    {
        return new Envelope(null, null, null);
    }

    public static Envelope Error(string? exceptionMessage, string? exceptionStackTrace, string? exceptionType, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    {
        return new Envelope(exceptionMessage, exceptionStackTrace, exceptionType, httpStatusCode);
    }

    public void SetExceptionStackTraceToNull()
    {
        ExceptionStackTrace = null;
    }
}