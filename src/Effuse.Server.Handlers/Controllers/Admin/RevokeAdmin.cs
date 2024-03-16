using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Delete, "/api/v1/admin-users/{userid}")]
public class RevokeAdmin(AdminService admin) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var userId = props.PathParameters["userid"];
    await admin.RevokeUserAdmin(token, Guid.Parse(userId));

    return new(204, new
    {
      Message = "Success"
    });
  }
}
