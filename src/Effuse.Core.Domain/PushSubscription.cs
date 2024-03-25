namespace Effuse.Core.Domain;

public class PushSubscription(string endpoint, DateTime expires, IDictionary<string, string> keys)
{
  public string Endpoint => endpoint;

  public DateTime Expires => expires;

  public IReadOnlyDictionary<string, string> Keys => keys.AsReadOnly();

  public string? ApplicationServerKey => this.Keys.ContainsKey("applicationServerKey")
    ? this.Keys["applicationServerKey"]
    : null;

  public bool UserVisibleOnly => Keys.ContainsKey("userVisibleOnly") && this.Keys["userVisibleOnly"] == "true";
}
