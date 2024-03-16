using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers;

[Route(Method.Get, "/api/v1/channels/{channelid}/users")]
public class ChannelUsers(ChannelsService channels) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);
    var channelId = props.PathParameters["channelid"];

    var users = await channels.GetChannelUsers(token, Guid.Parse(channelId));
    return new(200, users.Select(u => new
    {
      UserId = u.UserId.ToString(),
      u.MayRead,
      u.MayWrite
    }));
  }
}
