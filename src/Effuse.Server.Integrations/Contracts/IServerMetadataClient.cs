namespace Effuse.Server.Integrations.Contracts;

public struct ServerMetadata
{
  public string Name { get; set; }

  public string IconBase64 { get; set; }

  public string IconMimeType { get; set; }
}

public interface IServerMetadataClient
{
  Task UpdateMetadata(string name, string iconBase64, string iconMimeType);

  Task<ServerMetadata> GetServerMetadata();
}
