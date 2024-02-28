using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Services;
using Effuse.Core.Utilities;

namespace Effuse.SSO.Handlers;

[Route(Method.Get, "/api/v1/auth/refresh-token")]
public class Refresh(AuthService authService) : IHandler
{
  private struct Response
  {
    public string AdminToken { get; set; }

    public string ServerToken { get; set; }

    public string RefreshToken { get; set; }

    public string Expires { get; set; }

    public string UserId { get; set; }
  }

  private readonly AuthService authService = authService;

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.QueryParameters["token"];

    if (token == null || token == string.Empty)
      return new(403);

    var response = await this.authService.Refresh(token);

    return new(200, new Response()
    {
      AdminToken = response.UserToken,
      ServerToken = response.ServerToken,
      UserId = response.UserId.ToString(),
      RefreshToken = response.RefreshToken,
      Expires = response.Expires.ToISOString()
    });
  }
}
