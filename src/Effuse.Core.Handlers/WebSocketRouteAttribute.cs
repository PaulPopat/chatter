namespace Effuse.Core.Handlers;

[AttributeUsage(AttributeTargets.Class)]
public class WebSocketRouteAttribute(string endpoint) : Attribute
{
  public string Endpoint { get; } = endpoint;
}
