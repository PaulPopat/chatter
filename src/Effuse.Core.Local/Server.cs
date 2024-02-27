using Unity;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Integration;
using System.Reflection;
using WebSocketSharp.Server;
using Effuse.Core.Handlers;

namespace Effuse.Core.Local;

public class Server(int port, UnityContainer container, Assembly assembly)
{
  private readonly HttpServer server = new(port);
  private readonly UnityContainer container = container;
  private readonly IEnumerable<Route> routes = Route.FromAssembly(assembly);

  private async void Process(object? sender, HttpRequestEventArgs e)
  {
    var req = e.Request;
    var res = e.Response;

    var connectionId = Guid.NewGuid();
    Console.WriteLine($"Handling {connectionId} {req.HttpMethod}: {req.Url?.AbsolutePath}");

    try
    {

      if (req.Url == null)
      {
        await res.ApplyResponse(new(404));
        return;
      }

      var route = this.routes.First(h => h.Matches(req.Url.AbsolutePath, req.HttpMethod));
      var handler = (IHandler)container.Resolve(route.Handler);
      var response = await handler.Handle(await req.HandlerProps(route, connectionId));

      await res.ApplyResponse(response);
    }
    catch (Exception error)
    {
      if (error is AuthException)
      {
        await res.ApplyResponse(new(403));
      }
      else
      {
        Console.WriteLine(error);
        await res.ApplyResponse(new(500));
      }
    }

    Console.WriteLine($"Finished {connectionId}");
  }

  public void Start()
  {
    this.server.OnRequest += this.Process;

    foreach (var route in WebSocketRouteInstance.FromAssembly(assembly))
    {
      this.server.AddWebSocketService(
        route.Endpoint,
        () => new WebSocketHandler((IWebSocketHandler)container.Resolve(route.Type)));
    }

    this.server.Start();

    if (this.server.IsListening)
    {
      Console.WriteLine("Listening on port {0}:", this.server.Port);
    }

    Console.WriteLine("\nPress Enter key to stop the server...");
    Console.ReadLine();

    this.server.Stop();
  }
}
