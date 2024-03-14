using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers;

[WebSocketRoute("/ws/chat/{channelid}")]
public class Chat(Messaging messaging) : IWebSocketHandler
{
  public async Task OnClose(string connectionId)
  {
    await messaging.Disconnect(connectionId);
  }

  public async Task OnConnect(HandlerProps props)
  {
    var token = props.QueryParameters["token"];
    var channelId = props.PathParameters["channelid"];
    await messaging.ListenToChannel(token, Guid.Parse(channelId), props.ConnectionId);
  }
}
