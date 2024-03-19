using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Get, "/api/v1/server/invite-link")]
public class InviteLink(AdminService adminService) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var publicUrl = props.QueryParameters["publicurl"];
    _ = bool.TryParse(props.QueryParameters["embedpassword"], out var embedPassword);
    _ = bool.TryParse(props.QueryParameters["admin"], out var admin);
    var link = await adminService.InviteLink(props.AuthToken, publicUrl, embedPassword, admin);

    return new(200, new
    {
      Url = link.Href
    });
  }
}
