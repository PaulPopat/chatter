using Effuse.Core.Handlers.Contracts;
using WebSocketSharp.Server;
using Effuse.Core.Utilities;
using WebSocketSharp;
using System.Text.Json;

namespace Effuse.Core.Local;

public class WebSocketHandler : WebSocketBehavior
{
  private static readonly IDictionary<string, WebSocketHandler> connections = new Dictionary<string, WebSocketHandler>();

  public static IReadOnlyDictionary<string, WebSocketHandler> Connections => (IReadOnlyDictionary<string, WebSocketHandler>)connections;

  private readonly IWebSocketHandler handler;

  public WebSocketHandler(IWebSocketHandler handler)
  {
    this.handler = handler;
  }

  protected override void OnOpen()
  {
    connections[this.ID] = this;
    base.OnOpen();
    this.handler.OnConnect(new HandlerProps(
      this.Context.RequestUri.AbsolutePath,
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
    this.Send(response);
  }

  protected override void OnClose(CloseEventArgs e)
  {
    connections.Remove(this.ID);
    base.OnClose(e);
    this.handler.OnClose(this.ID);
  }

  protected override void OnError(WebSocketSharp.ErrorEventArgs e)
  {
    connections.Remove(this.ID);
    base.OnError(e);
    this.handler.OnClose(this.ID);
  }

  public void Send(object data)
  {
    base.Send(JsonSerializer.Serialize(data));
  }
}
