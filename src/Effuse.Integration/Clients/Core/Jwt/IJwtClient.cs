namespace Effuse.Integration.Clients.Jwt;

public interface IJwtClient
{
  Task<string> CreateJwt(string value, DateTime expires);

  Task<string> DecodeJwt(string jwt);
}