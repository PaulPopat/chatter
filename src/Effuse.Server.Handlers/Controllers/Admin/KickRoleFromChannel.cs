using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Delete, "/api/v1/roles/{roleId}/channels/{channelId}")]
public class KickRoleFromChannel(AdminService admin) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var channel = props.PathParameters["channelId"];
    var roleId = props.PathParameters["roleId"];

    await admin.KickRoleFromChannel(token, Guid.Parse(channel), Guid.Parse(roleId));

    return new(201, new
    {
      Message = "Success"
    });
  }
}
