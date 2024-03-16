using Unity;

namespace Effuse.Server.Services;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<AdminService>();
    container.RegisterType<AuthService>();
    container.RegisterType<ChannelsService>();
    container.RegisterType<MessagingService>();
    container.RegisterType<MetadataService>();
  }
}
