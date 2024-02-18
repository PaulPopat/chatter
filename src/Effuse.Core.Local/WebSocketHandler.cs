using Effuse.Core.Handlers.Contracts;
using WebSocketSharp.Server;
using Effuse.Core.Utilities;
using WebSocketSharp;
using System.Text.Json;

namespace Effuse.Core.Local;

public class WebSocketHandler : WebSocketBehavior
{
  private readonly IWebSocketHandler handler;

  public WebSocketHandler(IWebSocketHandler handler)
  {
    this.handler = handler;
  }

  protected override void OnOpen()
  {
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
