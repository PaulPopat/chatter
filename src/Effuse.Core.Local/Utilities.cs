using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using WebSocketSharp.Net;

namespace Effuse.Core.Local;

public static class Utilities
{
  public static Dictionary<string, string> GetQueryString(this Uri? uri)
  {
    if (uri == null) return new Dictionary<string, string>();

    var queryString = uri.Query;
    var queryParts = System.Web.HttpUtility.ParseQueryString(queryString);
    return queryParts.ToDictionary();
  }

  public static Dictionary<string, string> ToDictionary(this NameValueCollection? collection)
  {
    var result = new Dictionary<string, string>();

    if (collection == null) return result;

    foreach (var key in collection.AllKeys)
    {
      if (key == null) continue;
      var input = collection[key];
      if (input is string s)
        result[key] = s;
    }

    return result;
  }

  public static async Task<string?> GetBody(this HttpListenerRequest request)
  {
    if (!request.HasEntityBody)
    {
      return null;
    }

    using Stream body = request.InputStream;
    using var reader = new StreamReader(body, request.ContentEncoding);
    return await reader.ReadToEndAsync();
  }

  public static async Task<HandlerProps> HandlerProps(this HttpListenerRequest req, Route route, Guid connectionId)
  {
    if (req.Url == null) throw new Exception("This should not be reachable");

    return new HandlerProps(
      path: req.Url.AbsolutePath,
      method: req.HttpMethod,
      connectionId: connectionId.ToString(),
      pathParameters: route.PathParameters(req.Url.AbsolutePath),
      queryParameter: req.Url.GetQueryString().ToLowerCaseKeys(),
      headers: req.Headers.ToDictionary().ToLowerCaseKeys(),
      body: await req.GetBody() ?? string.Empty
    );
  }

  public static async Task ApplyResponse(this HttpListenerResponse? res, HandlerResponse response)
  {
    if (res == null) return;
    var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response.Body ?? new { }));
    res.ContentType = "application/json";
    res.ContentEncoding = Encoding.UTF8;
    res.ContentLength64 = data.LongLength;

    res.StatusCode = response.StatusCode;

    await res.OutputStream.WriteAsync(data);
    res.Close();
  }
}
