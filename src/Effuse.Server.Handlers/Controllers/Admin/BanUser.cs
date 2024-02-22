using Effuse.Core.Handlers.Contracts;
using Service = Effuse.Server.Services.Admin;

namespace Effuse.Server.Handlers.Controllers.Admin;

public class BanUser : IHandler
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

  public BanUser(Service admin)
  {
    this.admin = admin;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    await this.admin.BanUser(token, Guid.Parse(props.Body<Request>().UserId));

    return new(201, new Response
    {
      Message = "Success"
    });
  }
}
