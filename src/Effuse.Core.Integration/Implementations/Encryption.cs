using System.Text;
using System.Security.Cryptography;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Integration.Implementations;

public class Encryption : IEncryption
{
  private readonly IParameters parameters;

  public Encryption(IParameters parameters)
  {
    this.parameters = parameters;
  }

  private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

  public async Task<string> Encrypt(string clearText)
  {
    var passPhrase = await this.parameters.GetParameter("ENCRYPTION_PASSPHRASE");
    var clearBytes = Encoding.UTF8.GetBytes(clearText);
    using (var encryptor = Aes.Create())
    {
      var pdb = new Rfc2898DeriveBytes(passPhrase, Salt);
      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      using var ms = new MemoryStream();
      using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
      {
        cs.Write(clearBytes, 0, clearBytes.Length);
        cs.Close();
      }
      clearText = Convert.ToHexString(ms.ToArray());
    }
    return clearText;
  }

  public async Task<string> Decrypt(string cipherText)
  {
    var passPhrase = await this.parameters.GetParameter("ENCRYPTION_PASSPHRASE");
    cipherText = cipherText.Replace(" ", "+");
    var cipherBytes = Convert.FromHexString(cipherText);
    using (var encryptor = Aes.Create())
    {
      var pdb = new Rfc2898DeriveBytes(passPhrase, Salt);
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
