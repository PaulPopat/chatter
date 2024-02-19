namespace Effuse.Core.Integration.Contracts;

public enum ParameterName
{
  ENCRYPTION_PASSPHRASE,
  JWT_CERTIFICATE,
  JWT_SECRET,
  SSO_BASE_URL
}

public interface IParameters
{
  Task<string> GetParameter(ParameterName name);
}