namespace Effuse.Core.Handlers;

[AttributeUsage(AttributeTargets.Class)]
public class RouteAttribute(Method method, string endpoint) : Attribute
{
  public Method Method { get; } = method;

  public string Endpoint { get; } = endpoint;
}
