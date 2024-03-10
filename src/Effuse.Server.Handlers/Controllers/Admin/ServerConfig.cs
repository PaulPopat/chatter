using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers;

[Route(Method.Put, "/api/v1/server/metadata")]
public class ServerConfig(Metadata metadata) : IHandler
{
  private struct Form
  {
    public string ServerName { get; set; }

    public string IconBase64 { get; set; }

    public string IconMimeType { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null || token == string.Empty) return new (403);
    var data = props.Body<Form>();

    await metadata.UpdateMetadata(token, data.ServerName, data.IconBase64, data.IconMimeType);

    return new (200);
  }
}
