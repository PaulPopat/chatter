using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Get, "/api/v1/users/{userid}/permissions")]
public class GetUserPermissions(AdminService admin) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null || token == string.Empty)
      return new(403, new { Message = "Error" });

    var userId = props.PathParameters["userid"];
    var data = await admin.GetUserPolicies(token, Guid.Parse(userId));

    return new(200, data.Select(p => new
    {
      p.ChannelId,
      p.Write
    }));
  }
}
