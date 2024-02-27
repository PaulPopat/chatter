using Effuse.SSO.Integration.Clients.User;
using Unity;

namespace Effuse.SSO.Integration;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<IUserClient, DbUserClient>();
  }
}
