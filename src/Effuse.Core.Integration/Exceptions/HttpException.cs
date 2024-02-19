namespace Effuse.Core.Integration.Exceptions;

public class HttpException : Exception
{
  public HttpException(int statusCode, string reason)
  {
    StatusCode = statusCode;
    Reason = reason;
  }

  public int StatusCode { get; }

  public string Reason { get; }
}
