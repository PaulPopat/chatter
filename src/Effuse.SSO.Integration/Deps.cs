using Effuse.SSO.Integration.Clients.PushSubscription;
using Effuse.SSO.Integration.Clients.User;
using Effuse.SSO.Integration.Server;
using Unity;

namespace Effuse.SSO.Integration;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<IUserClient, DbUserClient>();
    container.RegisterType<IServerClient, HttpServerClient>();
    container.RegisterType<IPushSubscriptionClient, PushSubscriptionClient>();
  }
}
