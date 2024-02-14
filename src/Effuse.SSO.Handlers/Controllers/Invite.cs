using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.Invite;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

public class Invite : IHandler
{
  private readonly AuthService authService;

  public Invite(AuthService authService)
  {
    this.authService = authService;
  }


  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var email = props.QueryParameters["Email"];

    if (email == null) return new (400);

    return new(200, new InviteResponse()
    {
      Code = await this.authService.CreateInvite(email)
    });
  }
}
