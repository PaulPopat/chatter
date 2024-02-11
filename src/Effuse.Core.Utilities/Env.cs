using System.Globalization;

namespace Effuse.Core.Utilities;

public static class Env
{
  public static string GetEnv(string name)
  {
    return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Required environment variable {name} is not set");
  }
}