using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Post, "/api/v1/roles/{roleId}/channels")]
public class AddRoleToChannel(AdminService admin) : IHandler
{
  private struct Request
  {
    public string ChannelId { get; set; }

    public bool AllowWrite { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var data = props.Body<Request>();
    var token = props.AuthToken;
    if (token == null) return new(403);

    var roleId = props.PathParameters["roleId"];

    await admin.AddRoleToChannel(token, Guid.Parse(data.ChannelId), Guid.Parse(roleId), data.AllowWrite);

    return new(201, new
    {
      Message = "Success"
    });
  }
}
