using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;
using Effuse.Core.Utilities;

namespace Effuse.Server.Handlers;

[Route(Method.Get, "/api/v1/channels/{channelid}/messages")]
public class ChatBacklog(Messaging messaging) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var offset = props.QueryParameters["offset"];
    var channelId = props.PathParameters["channelid"];

    var response = await messaging.GetBackLog(token, Guid.Parse(channelId), long.Parse(offset));

    return new(200, response.Select(m => new
    {
      m.Text,
      When = m.When.ToISOString(),
      Who = m.UserId.ToString()
    }).ToList());
  }
}
