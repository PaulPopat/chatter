using System.Net;
using Unity;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Integration;
using System.Reflection;

namespace Effuse.Core.Local;

public class Server
{
  private readonly HttpListener listener;
  private readonly string url;
  private readonly UnityContainer container;
  private readonly IEnumerable<Route> routes;

  public Server(int port, UnityContainer container, Assembly assembly)
  {
    this.listener = new HttpListener();
    this.url = $"http://*:{port}/";
    this.container = container;
    this.routes = Route.FromAssembly(assembly);
  }


  private async Task HandleIncomingConnections()
  {
    while (true)
    {
      var ctx = await listener.GetContextAsync();

      var req = ctx.Request;
      var res = ctx.Response;

      var connectionId = Guid.NewGuid();
      Console.WriteLine($"Handling {connectionId} {req.HttpMethod}: {req.Url?.AbsolutePath}");

      try
      {

        if (req.Url == null)
        {
          await res.ApplyResponse(new(404));
          continue;
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
  }


  public async Task StartServer()
  {
    listener.Prefixes.Add(url);
    listener.Start();
    Console.WriteLine("Listening for connections on {0}", url);

    await HandleIncomingConnections();

    listener.Close();
  }
}
