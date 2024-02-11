using System.Text.Json;

namespace Effuse.Core.Handlers.Contracts;

public class HandlerProps
{
  private readonly string body;

  public HandlerProps
    (
      string path,
      string method,
      string connectionId,
      IDictionary<string, string> pathParameters,
      IDictionary<string, string> queryParameter,
      IDictionary<string, string> headers,
      string body = ""
    )
  {
    this.Path = path;
    this.Method = method;
    this.ConnectionId = connectionId;
    this.PathParameters = pathParameters;
    this.QueryParameters = queryParameter;
    this.Headers = headers;
    this.body = body;
  }

  public string Path { get; }

  public IDictionary<string, string> PathParameters { get; }

  public IDictionary<string, string> Headers { get; }

  public IDictionary<string, string> QueryParameters { get; }

  public string Method { get; }

  public string ConnectionId { get; }

  public TBody? Body<TBody>() {
    if (this.body == string.Empty) return default;

    return JsonSerializer.Deserialize<TBody>(this.body);
  }

  public string? AuthToken
  {
    get {
      if (!this.Headers.ContainsKey("Authorization")) return null;
      var authHeader = this.Headers["Authorization"];
      if (!authHeader.StartsWith("Bearer ")) return null;
      var token = authHeader.Replace("Bearer ", string.Empty);
      if (token == string.Empty) return null;
      return token;
    }
  }
}