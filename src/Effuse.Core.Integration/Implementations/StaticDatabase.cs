using System.Text.Json;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Integration.Implementations;

public class StaticDatabase(IStatic @static, IEncryption encryption) : IDatabase
{
  private async Task<string> ItemPath(string tableName, string primaryKey)
  {
    return $"database/{tableName}/{await encryption.Encrypt(primaryKey)}.json";
  }

  public async Task AddItem<T>(string tableName, string primaryKey, T item)
    where T : struct
  {
    var existing = await this.FindItem<T>(tableName, primaryKey);
    if (existing != null) throw new ConflictException();

    var json = JsonSerializer.Serialize(item);
    var encrypted = await encryption.Encrypt(json);
    await @static.UploadText(new StaticTextFile
    {
      Mime = "application/json",
      Data = encrypted,
      Name = await this.ItemPath(tableName, primaryKey)
    });
  }

  public async Task DeleteItem(string tableName, string primaryKey)
  {
    await @static.Delete(await this.ItemPath(tableName, primaryKey));
  }

  public async Task<TExpect?> FindItem<TExpect>(string tableName, string primaryKey)
    where TExpect : struct
  {
    try
    {
      var response = await @static.DownloadText(await this.ItemPath(tableName, primaryKey));
      if (response.Data == null || response.Data == string.Empty) return null;
      var decrypted = await encryption.Decrypt(response.Data);
      return JsonSerializer.Deserialize<TExpect>(decrypted);
    }
    catch
    {
      return null;
    }
  }

  public async Task<TExpect> GetItem<TExpect>(string tableName, string primaryKey)
    where TExpect : struct
  {
    return (await this.FindItem<TExpect>(tableName, primaryKey)) ?? throw new NotFoundException();
  }

  public async Task UpdateItem<T>(string tableName, string primaryKey, T item)
    where T : struct
  {
    if (!await this.Exists(tableName, primaryKey)) throw new NotFoundException();

    var json = JsonSerializer.Serialize(item);
    await @static.UploadText(new StaticTextFile
    {
      Mime = "application/json",
      Data = await encryption.Encrypt(json),
      Name = await this.ItemPath(tableName, primaryKey)
    });
  }

  public async Task<bool> Exists(string tableName, string primaryKey)
  {
    return await @static.Exists(await this.ItemPath(tableName, primaryKey));
  }
}
