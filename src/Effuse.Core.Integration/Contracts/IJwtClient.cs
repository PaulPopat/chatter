namespace Effuse.Core.Integration.Contracts;

public interface IJwtClient
{
  Task<string> CreateJwt<TData>(TData payload, TimeSpan expires);

  Task<TData> DecodeJwt<TData>(string jwt);
}
