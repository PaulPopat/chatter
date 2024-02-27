using System.Reflection;
using Effuse.Core.Handlers.Contracts;

namespace Effuse.Core.Handlers;

public class RouteInstance(Method method, string endpoint, Type type)
{
  public Method Method { get; } = method;

  public string Endpoint { get; } = endpoint;

  public Type Type { get; } = type;

  public static List<RouteInstance> FromAssembly(Assembly assembly)
  {
    return assembly.GetTypes()
      .Where(t => t.GetCustomAttribute(typeof(RouteAttribute), false) != null)
      .Select(t =>
      {
        if (!t.IsAssignableTo(typeof(IHandler))) throw new Exception("Only IHandlers may have a route");
        var route = t.GetCustomAttribute<RouteAttribute>(false) ?? throw new Exception("Should not be reachable");

        return new RouteInstance(route.Method, route.Endpoint, t);
      })
      .ToList();
  }
}
