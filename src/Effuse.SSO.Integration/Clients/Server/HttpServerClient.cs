
using System.Net;
using System.Text;
using System.Text.Json;
using Effuse.Core.Integration;
using Effuse.Core.Utilities;

namespace Effuse.SSO.Integration.Server;

public class HttpServerClient : IServerClient
{
  public async Task JoinServer(string serverUrl, string token, string password)
  {
    using var client = new HttpClient();

    var url = new Url(serverUrl, "/api/v1/users");

    var response = await client.PostAsync(
      url.Href,
      new StringContent(
        JsonSerializer.Serialize(new
        {
          ServerToken = token,
          Password = password
        }),
        Encoding.UTF8,
        "application/json"));

    if (response.StatusCode != HttpStatusCode.OK)
      throw new AuthException("Could not authenticate with server");
  }
}
