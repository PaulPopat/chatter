using Effuse.Server.Integrations.Contracts;
using Effuse.Server.Integrations.Implementations;
using Unity;

namespace Effuse.Server.Integrations;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<ISsoClient, HttpSsoClient>();
    container.RegisterType<IUserClient, DbUserClient>();
    container.RegisterType<IServerMetadataClient, ServerMetadataClient>();
  }
}
