using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models;
using Effuse.SSO.Handlers.Models.Register;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;


public class Register : IHandler<RegisterForm, RegisterResponse>
{
  private readonly AuthService authService;

  public Register(AuthService authService)
  {
    this.authService = authService;
  }

  public async Task<HandlerResponse<RegisterResponse>> Handle(HandlerProps<RegisterForm> props)
  {
    if (props.Body == null)
      return new(400, null);

    var response = await this.authService.Register(
      props.Body.Email,
      props.Body.Password,
      props.Body.InviteToken);

    return new(201, new RegisterResponse()
    {
      AdminToken = response.UserToken,
      ServerToken = response.ServerToken
    });
  }
}
