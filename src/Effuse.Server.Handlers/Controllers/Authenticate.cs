using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers;

[Route(Method.Get, "/api/v1/auth/token")]
public class Authenticate : IHandler
{
  private readonly Auth auth;

  public Authenticate(Auth auth)
  {
    this.auth = auth;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var ssoToken = props.QueryParameters["token"];

    var (token, isAdmin) = await this.auth.Authenticate(ssoToken, string.Empty);

    return new(200, new
    {
      LocalToken = token,
      IsAdmin = isAdmin
    });
  }
}
