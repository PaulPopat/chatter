using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Service = Effuse.Server.Services.Admin;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Post, "/api/v1/admin-users")]
public class GiveUserAdmin : IHandler
{
  private struct Request
  {
    public string UserId { get; set; }
  }

  private struct Response
  {
    public string Message { get; set; }
  }

  private readonly Service admin;

  public GiveUserAdmin(Service admin)
  {
    this.admin = admin;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    await this.admin.GiveUserAdmin(token, Guid.Parse(props.Body<Request>().UserId));

    return new(201, new Response
    {
      Message = "Success"
    });
  }
}
