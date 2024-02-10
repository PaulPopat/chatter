namespace Effuse.Core.Integration.Contracts;

public struct StaticFile
{
  public string Name { get; set; }

  public MemoryStream Data { get; set; }

  public string Mime { get; set; }
}

public interface IStatic
{
  Task Upload(StaticFile file);

  Task<StaticFile> Download(string name);
}