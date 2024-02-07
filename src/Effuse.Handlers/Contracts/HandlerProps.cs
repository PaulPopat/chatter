namespace Effuse.Handlers.Contracts;

public class HandlerProps<TBody>
{
  public HandlerProps
    (
      string path,
      string method,
      string connectionId,
      IDictionary<string, string> pathParameters,
      IDictionary<string, string> queryParameter,
      IDictionary<string, string> headers,
      TBody body
    )
  {
    this.Path = path;
    this.Method = method;
    this.ConnectionId = connectionId;
    this.PathParameters = pathParameters;
    this.QueryParameters = queryParameter;
    this.Headers = headers;
    this.Body = body;
  }

  public string Path { get; }

  public IDictionary<string, string> PathParameters { get; }

  public IDictionary<string, string> Headers { get; }

  public IDictionary<string, string> QueryParameters { get; }

  public string Method { get; }

  public string ConnectionId { get; }

  public TBody Body { get; }
}