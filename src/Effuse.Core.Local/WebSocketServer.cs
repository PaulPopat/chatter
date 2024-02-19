using Effuse.Core.Handlers.Contracts;
using Unity;
using BaseServer = WebSocketSharp.Server.WebSocketServer;

namespace Effuse.Core.Local;

public class WebSocketServer
{
  public WebSocketServer(int port, UnityContainer container, IEnumerable<Route> routes)
  {
    var server = new BaseServer(port);

    foreach (var route in routes)
    {
      server.AddWebSocketService(
        route.Path,
        () => new WebSocketHandler((IWebSocketHandler)container.Resolve(route.Handler)));
    }

    server.Start();
  }
}
