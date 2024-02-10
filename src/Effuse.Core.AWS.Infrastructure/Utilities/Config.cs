namespace Effuse.Core.AWS.Infrastructure.Utilities;

public static class Config
{
  public static string HandlersProject => "Effuse.AWS.Handlers";

  public static string AWSRegion => "eu-west-2";

  public static string AppPrefix => Environment.GetEnvironmentVariable("APP_PREFIX") ?? throw new Exception("App prefix is not defined");

  public static string ProjectPath(string path)
  {
    return Path.Combine(Directory.GetCurrentDirectory(), path);
  }
}