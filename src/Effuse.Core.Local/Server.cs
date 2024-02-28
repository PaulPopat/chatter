using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Integration;
using Effuse.Core.Utilities;
using Unity;

namespace Effuse.Core.Local;

public class Server
{
  private readonly HttpListener listener;
  private readonly IEnumerable<Route> routes;
  private readonly IEnumerable<Route> webSocketRoutes;
  private readonly UnityContainer container;

  private static readonly Dictionary<string, WebSocket> websockets = [];

  public static IReadOnlyDictionary<string, WebSocket> WebSockets => websockets;

  public Server(int port, UnityContainer container, Assembly assembly)
  {
    this.listener = new();
    this.listener.Prefixes.Add($"http://*:{port}/");
    this.routes = Route.FromAssembly(assembly);
    this.webSocketRoutes = Route.WebSocketsFromAssembly(assembly);
    this.container = container;
  }

  public async Task Start()
  {
    listener.Start();
    Console.WriteLine($"Listening at {string.Join(", ", this.listener.Prefixes)}");

    while (true)
    {
      var ctx = await listener.GetContextAsync();
      if (ctx.Request.IsWebSocketRequest)
      {
        ProcessWebSocketRequest(ctx);
      }
      else if (ctx.Request.HttpMethod.ToString().Equals("options", StringComparison.InvariantCultureIgnoreCase))
      {
        await ctx.Response.ApplyResponse(new(200));
      }
      else
      {
        ProcessHttpRequest(ctx);
      }
    }
  }

  private async void ProcessHttpRequest(HttpListenerContext ctx)
  {
    var req = ctx.Request;
    var res = ctx.Response;

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

  private async void ProcessWebSocketRequest(HttpListenerContext ctx)
  {
    WebSocketContext? webSocketContext = null;
    try
    {
      webSocketContext = await ctx.AcceptWebSocketAsync(subProtocol: null);
    }
    catch (Exception e)
    {
      ctx.Response.StatusCode = 500;
      ctx.Response.Close();
      Console.WriteLine("Exception: {0}", e);
      return;
    }

    var ws = webSocketContext.WebSocket;
    var connectionId = Guid.NewGuid();
    var connectionString = connectionId.ToString();
    websockets[connectionString] = ws;
    IWebSocketHandler? handler = null;

    try
    {
      var req = ctx.Request;
      var res = ctx.Response;

      if (req.Url == null)
      {
        await res.ApplyResponse(new(404));
        return;
      }

      var route = this.webSocketRoutes.First(h => h.Matches(req.Url.AbsolutePath, req.HttpMethod));
      handler = (IWebSocketHandler)container.Resolve(route.Handler);
      var props = await ctx.Request.HandlerProps(route, connectionId);

      await handler.OnConnect(props);

      while (ws.State == WebSocketState.Open)
      {
        try
        {
          var message = await ws.ReadMessage();

          var response = await handler.OnMessage(connectionString, message);
          await ws.SendJson(response);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          await ws.SendJson(new { Type = "Error", Message = "INTERNAL_ERROR" });
        }
      }

    }
    catch (Exception e)
    {
      Console.WriteLine(e);
    }
    finally
    {
      ws?.Dispose();
      if (handler != null)
        await handler.OnClose(connectionString);

      websockets.Remove(connectionString);
    }
  }
}
