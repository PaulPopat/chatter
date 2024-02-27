using Unity;

namespace Effuse.Server.Services;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<Admin>();
    container.RegisterType<Auth>();
    container.RegisterType<Channels>();
    container.RegisterType<Messaging>();
  }
}
