using Effuse.SSO.Handlers.Controllers;

namespace Effuse.SSO.Local;

public struct Route
{
  public HttpMethod Method { get; set; }

  public string Path { get; set; }

  public string Handler { get; set; }

  public readonly Type HandlerType
  {
    get
    {
      var asm = typeof(GetProfile).Assembly;
      return asm.GetType($"Effuse.SSO.Handlers.Controllers.{this.Handler}") ?? throw new Exception("Could not find handler");
    }
  }

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

  public readonly bool Matches(string url)
  {
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
}
