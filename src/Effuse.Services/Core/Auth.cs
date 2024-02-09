using Effuse.Integration.Clients.User;

namespace Effuse.Services.Core;

public class AuthService
{
  private readonly IUserClient userClient;

  public AuthService(IUserClient userClient)
  {
    this.userClient = userClient;
  }
}