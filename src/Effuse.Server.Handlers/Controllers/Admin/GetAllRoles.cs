using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers;

[Route(Method.Get, "/api/v1/roles")]
public class GetAllRoles(AdminService adminService) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var roles = await adminService.GetAllRoles(token);

    return new(201, roles.Select(r => new
    {
      r.RoleId,
      r.Name,
      r.Admin,
      Policies = r.Policies.Select(p => new
      {
        p.ChannelId,
        p.Write
      })
    }));
  }
}
