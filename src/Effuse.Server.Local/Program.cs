using System.Reflection;
using Effuse.Core.Local;
using Unity;

namespace Effuse.Server.Local;

class HttpServer
{
  public static void Main(string[] args)
  {
    var container = new UnityContainer();
    Effuse.Core.Integration.Deps.Register(container);
    Effuse.Core.Local.Deps.Register(container);
    Effuse.Server.Integrations.Deps.Register(container);
    Effuse.Server.Services.Deps.Register(container);
    Effuse.Server.Handlers.Deps.Register(container);

    var assembly = Assembly.Load("Effuse.Server.Handlers") ?? throw new Exception("Could not find server assembly");

    _ = new WebSocketServer(3003, container, assembly);

    new Effuse.Core.Local.Server(3002, container, assembly)
      .StartServer()
      .GetAwaiter()
      .GetResult();
  }
}
