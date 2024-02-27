using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Service = Effuse.Server.Services.Admin;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Post, "/api/v1/channels/{channelId}/users")]
public class AddUserToChannel : IHandler
{
  private struct Request
  {
    public string UserId { get; set; }

    public bool AllowWrite { get; set; }
  }

  private struct Response
  {
    public string Message { get; set; }
  }

  private readonly Service admin;

  public AddUserToChannel(Service admin)
  {
    this.admin = admin;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var data = props.Body<Request>();
    var token = props.AuthToken;
    if (token == null) return new(403);

    var channel = props.PathParameters["channelId"];

    await this.admin.AddUserToChannel(token, Guid.Parse(channel), Guid.Parse(data.UserId), data.AllowWrite);

    return new(201, new Response
    {
      Message = "Success"
    });
  }
}
