using System.Reflection;
using Effuse.Core.Handlers.Contracts;

namespace Effuse.Core.Handlers;

public class WebSocketRouteInstance(string endpoint, Type type)
{
  public string Endpoint { get; } = endpoint;

  public Type Type { get; } = type;

  public static List<WebSocketRouteInstance> FromAssembly(Assembly assembly)
  {
    return assembly.GetTypes()
      .Where(t => t.GetCustomAttribute(typeof(WebSocketRouteAttribute), false) != null)
      .Select(t =>
      {
        if (!t.IsAssignableTo(typeof(IWebSocketHandler))) throw new Exception("Only IWebSocketHandlers may have a route");
        var route = t.GetCustomAttribute<WebSocketRouteAttribute>(false) ?? throw new Exception("Should not be reachable");

        return new WebSocketRouteInstance(route.Endpoint, t);
      })
      .ToList();
  }
}
