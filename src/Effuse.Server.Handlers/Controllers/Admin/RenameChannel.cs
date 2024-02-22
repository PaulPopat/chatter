using Effuse.Core.Handlers.Contracts;
using Service = Effuse.Server.Services.Admin;

namespace Effuse.Server.Handlers.Controllers.Admin;

public class RenameChannel : IHandler
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

  public RenameChannel(Service admin)
  {
    this.admin = admin;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var data = props.Body<Request>();
    var token = props.AuthToken;
    if (token == null) return new(403);

    var channel = props.PathParameters["channelId"];

    var response = await this.admin.UpdateChannel(token, Guid.Parse(channel), data.Name, data.Public);

    return new(201, new Response
    {
      ChannelId = response.ChannelId.ToString(),
      Type = Enum.GetName(response.Type) ?? throw new Exception("Could not get enum name"),
      Name = response.Name,
      Public = response.Public
    });
  }
}
