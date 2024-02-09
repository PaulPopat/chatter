using Effuse.SSO.Integration.Clients.User;

namespace Effuse.SSO.Services;

public class AuthService
{
  private readonly IUserClient userClient;

  public AuthService(IUserClient userClient)
  {
    this.userClient = userClient;
  }
}