using System.Reflection;
using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Unity;
using BaseServer = WebSocketSharp.Server.WebSocketServer;

namespace Effuse.Core.Local;

public class WebSocketServer
{
  public WebSocketServer(int port, UnityContainer container, Assembly assembly)
  {
    var server = new BaseServer(port);

    foreach (var route in WebSocketRouteInstance.FromAssembly(assembly))
    {
      server.AddWebSocketService(
        route.Endpoint,
        () => new WebSocketHandler((IWebSocketHandler)container.Resolve(route.Type)));
    }

    server.Start();
  }
}
