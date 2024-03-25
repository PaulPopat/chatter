using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Base = Effuse.Server.Services.ChannelsService;

namespace Effuse.Server.Handlers.Controllers;

[Route(Method.Get, "/api/v1/channels")]
public class Channels : IHandler
{
  private struct Response
  {
    public string ChannelId { get; set; }

    public string Type { get; set; }

    public string Name { get; set; }

    public bool Public { get; set; }
  }

  private readonly Base channels;

  public Channels(Base channels)
  {
    this.channels = channels;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);

    var response = await this.channels.GetChannels(token);

    return new(200, response.Select(c => new Response
    {
      ChannelId = c.ChannelId.ToString(),
      Type = Enum.GetName(c.Type) ?? string.Empty,
      Name = c.Name,
      Public = c.Public
    }));
  }
}
