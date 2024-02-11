namespace Effuse.SSO.Integration.Clients.Jwt;

public interface IJwtClient
{
  Task<string> CreateJwt<TData>(TData payload, TimeSpan expires);

  Task<TData> DecodeJwt<TData>(string jwt);
}