using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Post, "/api/v1/admin-roles")]
public class GiveRoleAdmin(AdminService admin) : IHandler
{
  private struct Request
  {
    public string RoleId { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    await admin.GiveRoleAdmin(token, Guid.Parse(props.Body<Request>().RoleId));

    return new(201, new
    {
      Message = "Success"
    });
  }
}
