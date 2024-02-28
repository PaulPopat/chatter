using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Services;
using Effuse.Core.Utilities;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Post, "/api/v1/users")]
public class Register(AuthService authService) : IHandler
{
  private struct Response
  {
    public string AdminToken { get; set; }

    public string ServerToken { get; set; }

    public string RefreshToken { get; set; }

    public string Expires { get; set; }

    public string UserId { get; set; }
  }

  private class Form
  {
    public string UserName { get; set; } = "";

    public string Email { get; set; } = "";

    public string Password { get; set; } = "";
  }

  private readonly AuthService authService = authService;

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var body = props.Body<Form>();
    if (body == null)
      return new(400);

    var response = await this.authService.Register(
      body.UserName,
      body.Email,
      body.Password);

    return new(201, new Response()
    {
      AdminToken = response.UserToken,
      ServerToken = response.ServerToken,
      UserId = response.UserId.ToString(),
      RefreshToken = response.RefreshToken,
      Expires = response.Expires.ToISOString()
    });
  }
}
