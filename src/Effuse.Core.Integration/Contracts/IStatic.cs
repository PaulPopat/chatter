namespace Effuse.Core.Integration.Contracts;

public struct StaticFile
{
  public string Name { get; set; }

  public MemoryStream Data { get; set; }

  public string Mime { get; set; }
}

public struct StaticTextFile
{
  public string Name { get; set; }

  public string Data { get; set; }

  public string Mime { get; set; }
}

public interface IStatic
{
  Task Upload(StaticFile file);

  Task<StaticFile> Download(string name);

  Task UploadText(StaticTextFile file);

  Task<StaticTextFile> DownloadText(string name);

  Task Delete(string name);
}