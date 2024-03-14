using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers;

[Route(Method.Post, "/api/v1/users")]
public class JoinServer(Auth auth) : IHandler
{
  private struct Form
  {
    public string ServerToken { get; set; }

    public string Password { get; set; }
  }

  private readonly Auth auth = auth;


  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var form = props.Body<Form>();

    var (token, _) = await this.auth.Authenticate(form.ServerToken, form.Password);
    if (token == null || token == string.Empty)
      return new(403, new { Message = "Error" });

    return new(200, new { Message = "Success" });
  }
}
