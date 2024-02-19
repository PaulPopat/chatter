namespace Effuse.Core.Integration.Contracts;

public enum ParameterName
{
  ENCRYPTION_PASSPHRASE,
  JWT_CERTIFICATE,
  JWT_SECRET,
  SSO_BASE_URL,
  SERVER_PASSWORD
}

public interface IParameters
{
  Task<string> GetParameter(ParameterName name);
}