using System.Text.Json;
using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers;

[WebSocketRoute("/ws/chat")]
public class Chat : IWebSocketHandler
{
  private struct BacklogRequest
  {
    public long Offset { get; set; }
  }

  private struct MessageResponse
  {
    public string Type { get; set; }

    public string Text { get; set; }

    public string When { get; set; }

    public string Who { get; set; }
  }

  private struct UnknownResponse
  {
    public string Type { get; set; }

    public string Message { get; set; }
  }

  private readonly Messaging messaging;

  public Chat(Messaging messaging)
  {
    this.messaging = messaging;
  }

  public async Task OnClose(string connectionId)
  {
    await this.messaging.Disconnect(connectionId);
  }

  public async Task OnConnect(HandlerProps props)
  {
    var token = props.QueryParameters["token"];
    var channelId = props.QueryParameters["channelid"];
    await this.messaging.ListenToChannel(token, Guid.Parse(channelId), props.ConnectionId);
  }

  public async Task<object> OnMessage(string connectionId, string message)
  {
    try
    {
      var parts = message.Split(':');
      var command = parts[0];
      var data = string.Join(':', parts.Skip(1));

      switch (command)
      {
        case "backlog":
          var body = JsonSerializer.Deserialize<BacklogRequest>(data);
          var response = await this.messaging.GetBackLog(connectionId, body.Offset);
          return response.Select(m => new MessageResponse
          {
            Type = "Message",
            Text = m.Text,
            When = m.When.ToISOString(),
            Who = m.UserId.ToString()
          }).ToList();
        case "send":
          await this.messaging.PostMessage(connectionId, data);
          return new UnknownResponse { Type = "Info", Message = "MESSAGE_SENT" };
        default:
          return new UnknownResponse { Type = "Error", Message = "UNKNOWN_COMMAND" };
      }
    }
    catch (Exception err)
    {
      Console.WriteLine(err);
      return new UnknownResponse { Type = "Error", Message = "INTERNAL_ERROR" };
    }
  }
}
