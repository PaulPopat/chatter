using Effuse.SSO.Integration.Clients.User;
using Effuse.SSO.Integration.Server;

namespace Effuse.SSO.Services;

public class ServersService(IUserClient userClient, IServerClient serverClient, AuthService authService)
{
  private readonly IUserClient userClient = userClient;
  private readonly IServerClient serverClient = serverClient;
  private readonly AuthService authService = authService;

  public async Task JoinServer(string adminToken, string serverToken, string serverUrl, string password)
  {
    var userId = await this.authService.Verify(adminToken, UserAccess.Admin);

    var user = await this.userClient.GetUser(userId);

    await this.serverClient.JoinServer(serverUrl, serverToken, password);

    await this.userClient.UpdateUser(user.WithServer(serverUrl));
  }
}