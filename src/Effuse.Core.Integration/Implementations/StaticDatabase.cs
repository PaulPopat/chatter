using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Integration.Implementations;

public class StaticDatabase : IDatabase
{
  private readonly IStatic @static;
  private readonly IParameters parameters;

  public StaticDatabase(IStatic @static, IParameters parameters)
  {
    this.@static = @static;
    this.parameters = parameters;

    this.EncryptionKey = new(async () =>
      Convert.FromBase64String(await this.parameters.GetParameter("ENCRYPTION_KEY")));
    this.EncryptionIv = new(async () =>
      Convert.FromBase64String(await this.parameters.GetParameter("ENCRYPTION_IV")));
  }

  private readonly Lazy<Task<byte[]>> EncryptionKey;
  private readonly Lazy<Task<byte[]>> EncryptionIv;

  private async Task<string> ItemPath(string tableName, string primaryKey)
  {
    return $"database/{tableName}/{await this.Encrypt(primaryKey)}.json";
  }

  private async Task<ICryptoTransform> GetTransform()
  {
    return DES.Create().CreateEncryptor(await this.EncryptionKey.Value, await this.EncryptionIv.Value);
  }

  private async Task<string> Encrypt(string text)
  {
    var transform = await this.GetTransform();
    var inputbuffer = Encoding.UTF8.GetBytes(text);
    var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
    return Convert.ToHexString(outputBuffer);
  }

  private async Task<string> Decrypt(string text)
  {
    var transform = await this.GetTransform();
    var inputbuffer = Convert.FromHexString(text);
    var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
    return Encoding.UTF8.GetString(outputBuffer);
  }

  public async Task AddItem<T>(string tableName, string primaryKey, T item)
    where T : struct
  {
    var existing = await this.FindItem<T>(tableName, primaryKey);
    if (existing != null) throw new Exception("Primary key already found");

    var json = JsonSerializer.Serialize(item);
    var encrypted = await this.Encrypt(json);
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
      var decrypted = await this.Decrypt(response.Data);
      Console.WriteLine($"Decrypted JSON: {decrypted}");
      return JsonSerializer.Deserialize<TExpect>(decrypted);
    }
    catch (Exception error)
    {
      Console.WriteLine(error);
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
      Data = await this.Encrypt(json),
      Name = await this.ItemPath(tableName, primaryKey)
    });
  }
}
