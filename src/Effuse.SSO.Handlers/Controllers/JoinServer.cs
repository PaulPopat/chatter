using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Post, "/api/v1/user/servers")]
public class JoinServer(ServersService serversService) : IHandler
{
  private struct Form
  {
    public string ServerToken { get; set; }

    public string ServerUrl { get; set; }

    public string Password { get; set; }
  }

  private readonly ServersService serversService = serversService;

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);
    var body = props.Body<Form>();

    await this.serversService.JoinServer(token, body.ServerToken, body.ServerUrl, body.Password);

    return new(200, new
    {
      Success = true
    });
  }
}
