using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.Invite;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Get, "/api/v1/auth/invite")]
public class Invite : IHandler
{
  private readonly AuthService authService;

  public Invite(AuthService authService)
  {
    this.authService = authService;
  }


  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var email = props.QueryParameters["email"];

    if (email == null) return new(400);

    return new(200, new InviteResponse()
    {
      Code = await this.authService.CreateInvite(email)
    });
  }
}
