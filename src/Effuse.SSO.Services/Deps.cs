using Unity;

namespace Effuse.SSO.Services;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<AuthService>();
    container.RegisterType<ProfileService>();
    container.RegisterType<ServersService>();
  }
}
