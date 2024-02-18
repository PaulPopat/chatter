using System.Text.Json;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using Unity;
using WebSocketSharp;
using WebSocketSharp.Server;
using BaseServer = WebSocketSharp.Server.WebSocketServer;

namespace Effuse.Core.Local;

public class WebSocketServer<THandler> : WebSocketBehavior
  where THandler : IWebSocketHandler
{
  private static readonly Lazy<BaseServer> Server = new(() => new BaseServer(3001));

  private readonly IWebSocketHandler handler;
  private readonly string url;

  public WebSocketServer(UnityContainer container, string url)
  {
    this.handler = container.Resolve<THandler>();
    this.url = url;
    Server.Value.AddWebSocketService<WebSocketServer<THandler>>(url, () => this);
  }

  protected override void OnOpen()
  {
    base.OnOpen();
    this.handler.OnConnect(new HandlerProps(
      this.url,
      "GET",
      this.ID,
      new Dictionary<string, string>(),
      this.Context.RequestUri.GetQueryString(),
      this.Context.Headers.ToDictionary().ToLowerCaseKeys()));
  }

  protected override async void OnMessage(MessageEventArgs e)
  {
    base.OnMessage(e);
    var response = await this.handler.OnMessage(this.ID, e.Data);
    this.Send(JsonSerializer.Serialize(response));
  }

  protected override void OnClose(CloseEventArgs e)
  {
    base.OnClose(e);
    this.handler.OnClose(this.ID);
  }

  protected override void OnError(WebSocketSharp.ErrorEventArgs e)
  {
    base.OnError(e);
    this.handler.OnClose(this.ID);
  }
}
