using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers;

[Route(Method.Post, "/api/v1/roles")]
public class CreateRole(AdminService adminService) : IHandler
{
  private struct Request
  {
    public string Name { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var data = props.Body<Request>();
    var token = props.AuthToken;
    if (token == null) return new(403);

    var role = await adminService.CreateRole(token, data.Name);

    return new(201, new
    {
      RoleId = role.RoleId,
      Name = role.Name,
    });
  }
}
