using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers;

[Route(Method.Get, "/api/v1/server/metadata")]
public class ServerMetadata(MetadataService metadata) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    try
    {
      var response = await metadata.GetServerMetadata();
      return new(200, new
      {
        ServerName = response.Name,
        Icon = new
        {
          Base64Data = response.IconBase64,
          MimeType = response.IconMimeType
        }
      });
    }
    catch
    {
      return new(200, new
      {
        ServerName = "Unnamed Server",
        Icon = new
        {
          Base64Data = "",
          MimeType = "image/png"
        }
      });
    }
  }
}
