using System.Reflection;
using Effuse.Core.Local;
using Effuse.SSO.Handlers.Controllers;
using Unity;

namespace Effuse.SSO.Local;

class HttpServer
{
  public static void Main(string[] args)
  {
    var container = new UnityContainer();
    Effuse.Core.Integration.Deps.Register(container);
    Effuse.Core.Local.Deps.Register(container);
    Effuse.SSO.Integration.Deps.Register(container);
    Effuse.SSO.Services.Deps.Register(container);
    Effuse.SSO.Handlers.Deps.Register(container);

    var assembly = Assembly.Load("Effuse.SSO.Handlers") ?? throw new Exception("Could not find server assembly");

    new Server(3000, container, assembly).Start().GetAwaiter().GetResult();
  }
}
