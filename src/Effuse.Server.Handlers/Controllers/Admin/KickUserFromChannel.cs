using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Service = Effuse.Server.Services.Admin;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Delete, "/api/v1/channels/{channelId}/users/{userId}")]
public class KickUserFromChannel : IHandler
{
  private struct Response
  {
    public string Message { get; set; }
  }

  private readonly Service admin;

  public KickUserFromChannel(Service admin)
  {
    this.admin = admin;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var channel = props.PathParameters["channelId"];
    var userId = props.PathParameters["userId"];

    await this.admin.KickUserFromChannel(token, Guid.Parse(channel), Guid.Parse(userId));

    return new(201, new Response
    {
      Message = "Success"
    });
  }
}
