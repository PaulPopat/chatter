using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.Register;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;


public class Register : IHandler
{
  private readonly AuthService authService;

  public Register(AuthService authService)
  {
    this.authService = authService;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var body = props.Body<RegisterForm>();
    if (body == null)
      return new(400);

    var response = await this.authService.Register(
      body.UserName,
      body.Email,
      body.Password,
      body.InviteToken);

    return new(201, new RegisterResponse()
    {
      AdminToken = response.UserToken,
      ServerToken = response.ServerToken,
      UserId = response.UserId.ToString()
    });
  }
}
