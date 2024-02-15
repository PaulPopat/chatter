namespace Effuse.SSO.Tests;

public static class TestSetup
{
  public static void Env()
  {
    Environment.SetEnvironmentVariable("APP_PREFIX", "app");
    Environment.SetEnvironmentVariable("BUCKET_NAME", "testbucket");
    Environment.SetEnvironmentVariable("USER_TABLE_NAME", "usertable");
    Environment.SetEnvironmentVariable("USER_TABLE_EMAIL_INDEX", "userindex");
  }
}
