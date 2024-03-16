using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;
using Effuse.Core.Utilities;

namespace Effuse.Server.Handlers;

[Route(Method.Post, "/api/v1/channels/{channelid}/messages")]
public class Message(MessagingService messaging) : IHandler
{
  private struct Form
  {
    public string Text { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var channelId = props.PathParameters["channelid"];
    var body = props.Body<Form>();

    await messaging.PostMessage(token, Guid.Parse(channelId), body.Text);

    return new(200, new { Message = "Message Sent" });
  }
}
