using System.Net;
using Unity;
using Effuse.Core.Handlers.Contracts;

namespace Effuse.Core.Local;

public class Server
{
  private readonly HttpListener listener;
  private readonly string url;
  private readonly UnityContainer container;
  private readonly IEnumerable<Route> routes;

  public Server(int port, UnityContainer container, IEnumerable<Route> routes)
  {
    this.listener = new HttpListener();
    this.url = $"http://localhost:{port}/";
    this.container = container;
    this.routes = routes;
  }


  private async Task HandleIncomingConnections()
  {
    while (true)
    {
      var ctx = await listener.GetContextAsync();

      var req = ctx.Request;
      var res = ctx.Response;

      try
      {

        if (req.Url == null)
        {
          await res.ApplyResponse(new(404));
          continue;
        }

        var route = this.routes.First(h => h.Matches(req.Url.AbsolutePath, req.HttpMethod));
        var handler = (IHandler)container.Resolve(route.Handler);
        var response = await handler.Handle(await req.HandlerProps(route));

        await res.ApplyResponse(response);
      }
      catch (Exception err)
      {
        Console.WriteLine(err);
        await res.ApplyResponse(new(500));
      }
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
