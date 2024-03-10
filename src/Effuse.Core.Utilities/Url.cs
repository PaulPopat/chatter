using System.Web;

namespace Effuse.Core.Utilities;

public class Url(string baseUrl, string url, IDictionary<string, string>? parameters = null)
{
  private readonly string baseUrl = baseUrl;
  private readonly string url = url;
  private readonly IDictionary<string, string> parameters = parameters ?? new Dictionary<string, string>();

  public string Href
  {
    get
    {
      var url =
          (!this.baseUrl.EndsWith('/') ? this.baseUrl + '/' : this.baseUrl) +
          (this.url.StartsWith('/') ? this.url[1..] : this.url);
      Dictionary<string, string> query = [];

      foreach (var (key, value) in this.parameters)
      {
        if (url.Contains(':' + key))
        {
          url = url.Replace(':' + key, HttpUtility.UrlEncode(value));
        }
        else
        {
          query[key] = value;
        }
      }

      if (query.Count > 0)
      {
        var queryString = query.Select((pair) => $"{HttpUtility.UrlEncode(pair.Key)}={HttpUtility.UrlEncode(pair.Value)}");
        url += '?' + string.Join('&', queryString);
      }

      return url;
    }
  }
}
