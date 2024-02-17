namespace Effuse.SSO.Local;

class HttpServer
{
  public static void Main(string[] args)
  {
    new Server().StartServer().GetAwaiter().GetResult();
  }
}
