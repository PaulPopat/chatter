using System.Text;
using System.Security.Cryptography;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Integration.Implementations;

public class Encryption : IEncryption
{
  private readonly IParameters parameters;

  private readonly Lazy<Task<string>> passphrase;

  public Encryption(IParameters parameters)
  {
    this.parameters = parameters;

    this.passphrase = new(() => this.parameters.GetParameter(ParameterName.ENCRYPTION_PASSPHRASE));
  }

  private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

  private static readonly char[] Padding = { '=' };

  private static string ToBase64String(byte[] data)
  {
    return Convert.ToBase64String(data)
        .TrimEnd(Padding).Replace('+', '-').Replace('/', '_');
  }

  private static byte[] FromBase64String(string data)
  {
    var incoming = data.Replace('_', '/').Replace('-', '+');
    switch (data.Length % 4)
    {
      case 2: incoming += "=="; break;
      case 3: incoming += "="; break;
    }
    return Convert.FromBase64String(incoming);
  }
  public async Task<string> Encrypt(string clearText)
  {
    var clearBytes = Encoding.UTF8.GetBytes(clearText);
    using (var encryptor = Aes.Create())
    {
      var pdb = new Rfc2898DeriveBytes(await this.passphrase.Value, Salt);
      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      using var ms = new MemoryStream();
      using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
      {
        cs.Write(clearBytes, 0, clearBytes.Length);
        cs.Close();
      }
      clearText = ToBase64String(ms.ToArray());
    }
    return clearText;
  }

  public async Task<string> Decrypt(string cipherText)
  {
    cipherText = cipherText.Replace(" ", "+");
    var cipherBytes = FromBase64String(cipherText);
    using (var encryptor = Aes.Create())
    {
      var pdb = new Rfc2898DeriveBytes(await this.passphrase.Value, Salt);
      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      using var ms = new MemoryStream();
      using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
      {
        cs.Write(cipherBytes, 0, cipherBytes.Length);
        cs.Close();
      }
      cipherText = Encoding.UTF8.GetString(ms.ToArray());
    }
    return cipherText;
  }
}
