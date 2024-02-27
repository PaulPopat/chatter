using System.Reflection;
using Effuse.Core.Handlers;

namespace Effuse.Core.Local;

public struct Route
{
  public HttpMethod Method { get; set; }

  public string Path { get; set; }

  public Type Handler { get; set; }

  public readonly IDictionary<string, string> PathParameters(string url)
  {
    var result = new Dictionary<string, string>();
    var actualParts = url.Split('/');
    var expectedParts = this.Path.Split('/');

    for (var i = 0; i < actualParts.Length; i++)
    {
      var actual = actualParts[i];
      var expected = expectedParts[i];

      if (expected.StartsWith('{'))
      {
        var name = expected.Replace("{", string.Empty).Replace("}", string.Empty);
        result[name] = actual;
      }
    }

    return result;
  }

  public readonly bool Matches(string url, string method)
  {
    if (this.Method.ToString() != method) return false;
    var actualParts = url.Split('/');
    var expectedParts = this.Path.Split('/');

    if (actualParts.Length != expectedParts.Length) return false;

    for (var i = 0; i < actualParts.Length; i++)
    {
      var actual = actualParts[i];
      var expected = expectedParts[i];

      if (!expected.StartsWith('{'))
      {
        if (expected != actual) return false;
      }
    }

    return true;
  }

  public static List<Route> FromAssembly(Assembly assembly)
  {
    return RouteInstance.FromAssembly(assembly)
      .Select(route =>
      {
        var method = Enum.GetName(route.Method) ?? throw new Exception("No method");

        return new Route
        {
          Method = HttpMethod.Parse(method),
          Path = route.Endpoint,
          Handler = route.Type
        };
      })
      .ToList();
  }
}
