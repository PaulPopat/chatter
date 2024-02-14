using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.Login;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;


public class Login : IHandler
{
  private readonly AuthService authService;

  public Login(AuthService authService)
  {
    this.authService = authService;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var email = props.QueryParameters["Email"];
    var password = props.QueryParameters["Password"];

    var response = await this.authService.Login(email, password);

    return new(200, new LoginResponse()
    {
      AdminToken = response.UserToken,
      ServerToken = response.ServerToken,
      UserId = response.UserId.ToString()
    });
  }
}
