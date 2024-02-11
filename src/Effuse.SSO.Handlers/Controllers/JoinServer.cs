using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.Server;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;


public class JoinServer : IHandler
{
  private readonly ServersService serversService;
  private readonly AuthService authService;

  public JoinServer(ServersService serversService, AuthService authService)
  {
    this.serversService = serversService;
    this.authService = authService;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);
    var userId = await this.authService.Verify(token, UserAccess.Admin);

    await this.serversService.JoinServer(userId, props.Body<JoinServerForm>().ServerUrl);

    return new(200, new JoinServerResponse()
    {
      Success = true
    });
  }
}
