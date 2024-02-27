using Effuse.Core.Handlers.Contracts;
using WebSocketSharp.Server;
using Effuse.Core.Utilities;
using WebSocketSharp;
using System.Text.Json;

namespace Effuse.Core.Local;

public class WebSocketHandler(IWebSocketHandler handler) : WebSocketBehavior
{
  private static readonly IDictionary<string, WebSocketHandler> connections = new Dictionary<string, WebSocketHandler>();

  public static IReadOnlyDictionary<string, WebSocketHandler> Connections => (IReadOnlyDictionary<string, WebSocketHandler>)connections;

  private readonly IWebSocketHandler handler = handler;

  private Task? setup;

  protected override async void OnOpen()
  {
    try
    {
      connections[this.ID] = this;
      base.OnOpen();
      this.setup = this.handler
        .OnConnect(
          new HandlerProps(
            this.Context.RequestUri.AbsolutePath,
            "GET",
            this.ID,
            new Dictionary<string, string>(),
            this.Context.RequestUri.GetQueryString().ToLowerCaseKeys(),
            this.Context.Headers.ToDictionary().ToLowerCaseKeys()));
      await this.setup;
    }
    catch (Exception err)
    {
      Console.WriteLine(err);
      Sessions.CloseSession(ID);
    }
  }

  protected override async void OnMessage(MessageEventArgs e)
  {
    try
    {
      if (this.setup is not null)
        await this.setup;
      base.OnMessage(e);
      var response = await this.handler.OnMessage(this.ID, e.Data);
      this.Send(response);
    }
    catch (Exception err)
    {
      Console.WriteLine(err);
    }
  }

  protected override async void OnClose(CloseEventArgs e)
  {
    if (this.setup is not null)
      await this.setup;
    this.setup?.Wait();
    connections.Remove(this.ID);
    base.OnClose(e);
    await this.handler.OnClose(this.ID);
  }

  protected override async void OnError(WebSocketSharp.ErrorEventArgs e)
  {
    if (this.setup is not null)
      await this.setup;
    connections.Remove(this.ID);
    base.OnError(e);
    await this.handler.OnClose(this.ID);
  }

  public async void Send(object data)
  {
    if (this.setup is not null)
      await this.setup;
    base.Send(JsonSerializer.Serialize(data));
  }
}
