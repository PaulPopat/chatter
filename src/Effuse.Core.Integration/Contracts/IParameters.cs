namespace Effuse.Core.Integration.Contracts;

public enum ParameterName
{
  ENCRYPTION_PASSPHRASE,
  JWT_CERTIFICATE,
  JWT_SECRET,
}

public interface IParameters
{
  Task<string> GetParameter(ParameterName name);
}