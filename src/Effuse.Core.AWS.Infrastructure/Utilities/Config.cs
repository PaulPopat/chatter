using Effuse.Core.Utilities;

namespace Effuse.Core.AWS.Infrastructure.Utilities;

public static class Config
{
  public static string HandlersProject(string area) {
    return $"Effuse.{area}.AWS.Handlers";
  }

  public static string AWSRegion => Env.GetEnv("AWS_REGION");

  public static string AWSAccount => Env.GetEnv("AWS_ACCOUNT");

  public static string AppPrefix => Env.GetEnv("APP_PREFIX");

  public static string ProjectPath(string path)
  {
    return Path.Combine(Directory.GetCurrentDirectory(), path);
  }
}