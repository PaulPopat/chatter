using Effuse.SSO.Integration.Clients.User;

namespace Effuse.SSO.Services;

public class ServersService
{
  private readonly IUserClient userClient;

  public ServersService(IUserClient userClient)
  {
    this.userClient = userClient;
  }

  public async Task JoinServer(Guid userId, string serverUrl)
  {
    var user = await this.userClient.GetUser(userId);

    await this.userClient.UpdateUser(user.WithServer(serverUrl));
  }
}