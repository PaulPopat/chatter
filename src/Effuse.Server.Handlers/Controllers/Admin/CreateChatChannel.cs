using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Service = Effuse.Server.Services.Admin;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Post, "/api/v1/channels")]
public class CreateChatChannel : IHandler
{
  private struct Request
  {
    public string Name { get; set; }

    public bool Public { get; set; }
  }

  private struct Response
  {
    public string ChannelId { get; set; }

    public string Type { get; set; }

    public string Name { get; set; }

    public bool Public { get; set; }
  }

  private readonly Service admin;

  public CreateChatChannel(Service admin)
  {
    this.admin = admin;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var data = props.Body<Request>();
    var token = props.AuthToken;
    if (token == null) return new(403);

    var response = await this.admin.CreateChatChannel(token, data.Name, data.Public);

    return new(201, new Response
    {
      ChannelId = response.ChannelId.ToString(),
      Type = Enum.GetName(response.Type) ?? throw new Exception("Could not get enum name"),
      Name = response.Name,
      Public = response.Public
    });
  }
}
