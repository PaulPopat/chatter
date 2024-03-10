namespace Effuse.SSO.Integration.Server;

public interface IServerClient
{
  Task JoinServer(string serverUrl, string token, string password);
}
