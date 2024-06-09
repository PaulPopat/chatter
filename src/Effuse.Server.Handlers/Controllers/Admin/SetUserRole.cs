using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Put, "/api/v1/users/{userId}/role")]
public class SetUserRole(AdminService admin) : IHandler
{
  private struct Request
  {
    public string RoleId { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var body = props.Body<Request>();
    var userId = props.PathParameters["userId"];
    var token = props.AuthToken;
    if (token == null) return new(403);

    await admin.SetUserRole(token, Guid.Parse(userId), Guid.Parse(body.RoleId));

    return new(200, new
    {
      Message = "Success"
    });
  }
}
