using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;

namespace Effuse.Core.Local.Integrations;

public class DiskStatics : IStatic
{
  private static string BaseDir => Env.GetEnv("DATA_DIR");

  private static string Location(string name)
  {
    return Path.Combine(BaseDir, name);
  }

  private static string MimeLocation(string name)
  {
    return Path.Combine(BaseDir, name + ".mime");
  }

  public Task Delete(string name)
  {
    File.Delete(Location(name));
    File.Delete(MimeLocation(name));
    return new Task(() => {});
  }

  public async Task<StaticFile> Download(string name)
  {
    return new StaticFile
    {
      Name = name,
      Data = new MemoryStream(await File.ReadAllBytesAsync(Location(name))),
      Mime = await File.ReadAllTextAsync(MimeLocation(name))
    };
  }

  public async Task<StaticTextFile> DownloadText(string name)
  {
    return new StaticTextFile
    {
      Name = name,
      Data = await File.ReadAllTextAsync(Location(name)),
      Mime = await File.ReadAllTextAsync(MimeLocation(name))
    };
  }

  public async Task Upload(StaticFile file)
  {
    Directory.CreateDirectory(
      Path.GetDirectoryName(Location(file.Name))
      ?? throw new Exception("Could not resolve directory name"));
    await File.WriteAllBytesAsync(Location(file.Name), file.Data.ToArray());
    await File.WriteAllTextAsync(MimeLocation(file.Name), file.Mime);
  }

  public async Task UploadText(StaticTextFile file)
  {
    Directory.CreateDirectory(
      Path.GetDirectoryName(Location(file.Name))
      ?? throw new Exception("Could not resolve directory name"));
    await File.WriteAllTextAsync(Location(file.Name), file.Data);
    await File.WriteAllTextAsync(MimeLocation(file.Name), file.Mime);
  }
}
