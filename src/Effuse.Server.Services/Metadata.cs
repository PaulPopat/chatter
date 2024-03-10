using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class Metadata(IServerMetadataClient serverMetadataClient, Auth auth)
{
  public async Task UpdateMetadata(string localToken, string name, string iconBase64, string iconMimeType)
  {
    await auth.RequireAdmin(localToken);
    await serverMetadataClient.UpdateMetadata(name, iconBase64, iconMimeType);
  }

  public async Task<ServerMetadata> GetServerMetadata()
  {
    return await serverMetadataClient.GetServerMetadata();
  }
}
