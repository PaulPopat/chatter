using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Get, "/api/v1/auth/token")]
public class Login(AuthService authService) : IHandler
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
    var email = props.QueryParameters["email"];
    var password = props.QueryParameters["password"];

    var response = await this.authService.Login(email, password);

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
