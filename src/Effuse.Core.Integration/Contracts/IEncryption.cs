namespace Effuse.Core.Integration.Contracts;

public interface IEncryption
{
  Task<string> Encrypt(string text);
  Task<string> Decrypt(string text);
}
