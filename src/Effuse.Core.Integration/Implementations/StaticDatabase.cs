using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Integration.Implementations;

public class StaticDatabase : IDatabase
{
  private readonly IStatic @static;
  private readonly IEncryption encryption;

  public StaticDatabase(IStatic @static, IEncryption encryption)
  {
    this.@static = @static;
    this.encryption = encryption;
  }

  private async Task<string> ItemPath(string tableName, string primaryKey)
  {
    return $"database/{tableName}/{await this.encryption.Encrypt(primaryKey)}.json";
  }

  public async Task AddItem<T>(string tableName, string primaryKey, T item)
    where T : struct
  {
    var existing = await this.FindItem<T>(tableName, primaryKey);
    if (existing != null) throw new Exception("Primary key already found");

    var json = JsonSerializer.Serialize(item);
    var encrypted = await this.encryption.Encrypt(json);
    await this.@static.UploadText(new StaticTextFile
    {
      Mime = "application/json",
      Data = encrypted,
      Name = await this.ItemPath(tableName, primaryKey)
    });
  }

  public Task DeleteItem(string tableName, string primaryKey)
  {
    throw new NotImplementedException();
  }

  public async Task<TExpect?> FindItem<TExpect>(string tableName, string primaryKey)
    where TExpect : struct
  {
    try
    {
      var response = await this.@static.DownloadText(await this.ItemPath(tableName, primaryKey));
      if (response.Data == null || response.Data == string.Empty) return null;
      var decrypted = await this.encryption.Decrypt(response.Data);
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
    return (await this.FindItem<TExpect>(tableName, primaryKey)) ?? throw new Exception("Could not find item");
  }

  public async Task UpdateItem<T>(string tableName, string primaryKey, T item)
    where T : struct
  {
    var existing = await this.FindItem<T>(tableName, primaryKey);
    if (existing == null) throw new Exception("Could not find item");

    var json = JsonSerializer.Serialize(item);
    await this.@static.UploadText(new StaticTextFile
    {
      Mime = "application/json",
      Data = await this.encryption.Encrypt(json),
      Name = await this.ItemPath(tableName, primaryKey)
    });
  }
}
