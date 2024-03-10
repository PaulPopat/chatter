using Effuse.Core.Integration.Contracts;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Integrations.Implementations;

public class ServerMetadataClient(IDatabase database) : IServerMetadataClient
{
  private static string TableName => "ServerMetadata";
  private static string TableItem => "Main";

  public Task<ServerMetadata> GetServerMetadata()
  {
    return database.GetItem<ServerMetadata>(TableName, TableItem);
  }

  public async Task UpdateMetadata(string name, string iconBase64, string iconMimeType)
  {
    try
    {
      await database.AddItem(TableName, TableItem, new ServerMetadata
      {
        Name = name,
        IconBase64 = iconBase64,
        IconMimeType = iconMimeType
      });
    }
    catch
    {
      await database.UpdateItem(TableName, TableItem, new ServerMetadata
      {
        Name = name,
        IconBase64 = iconBase64,
        IconMimeType = iconMimeType
      });
    }
  }
}
