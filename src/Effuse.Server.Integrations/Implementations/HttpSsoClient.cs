using System.Net.Http.Json;
using System.Web;
using Effuse.Core.Integration.Contracts;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Integrations;

public class HttpSsoClient : ISsoClient
{
  private struct ValidateTokenResponse
  {
    public string UserId { get; set; }
  }

  private readonly IParameters parameters;

  public HttpSsoClient(IParameters parameters)
  {
    this.parameters = parameters;
  }

  public async Task<Guid> GetUserId(string token)
  {
    var baseUrl = await this.parameters.GetParameter(ParameterName.SSO_BASE_URL);

    using var http = new HttpClient();
    var uri = new UriBuilder(new Uri(new Uri(baseUrl), "/api/v1/auth/user"));
    var query = HttpUtility.ParseQueryString(uri.Query);
    query["token"] = token;
    uri.Query = query.ToString();

    var response = await http.GetFromJsonAsync<ValidateTokenResponse>(uri.Uri);

    return Guid.Parse(response.UserId);
  }
}
