using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.GetUserFromToken;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

public class GetUserFromToken : IHandler
{
  private readonly AuthService authService;

  public GetUserFromToken(AuthService authService)
  {
    this.authService = authService;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.QueryParameters["Token"];
    if (token == null || token == string.Empty)
      return new(400);

    var userId = await this.authService.Verify(token, UserAccess.Identify);
    
    return new (200, new GetUserFromTokenResponse
    {
      UserId = userId.ToString()
    });
  }
}
