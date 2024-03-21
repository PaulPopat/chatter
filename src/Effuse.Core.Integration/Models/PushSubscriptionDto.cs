namespace Effuse.Core.Integration;

public struct PushSubscriptionDto
{
  public string Endpoint { get; set; }

  public string Expires { get; set; }

  public Dictionary<string, string> Keys { get; set; }
}
