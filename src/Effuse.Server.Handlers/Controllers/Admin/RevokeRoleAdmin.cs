using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Delete, "/api/v1/admin-roles/{roleId}")]
public class RevokeRoleAdmin(AdminService admin) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var roleId = props.PathParameters["roleId"];
    await admin.RevokeRoleAdmin(token, Guid.Parse(roleId));

    return new(204, new
    {
      Message = "Success"
    });
  }
}
