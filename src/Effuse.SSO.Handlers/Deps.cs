using Effuse.SSO.Handlers.Controllers;
using Unity;

namespace Effuse.SSO.Handlers;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<GetProfile>();
    container.RegisterType<GetPublicProfile>();
    container.RegisterType<HeartBeat>();
    container.RegisterType<JoinServer>();
    container.RegisterType<Login>();
    container.RegisterType<Register>();
    container.RegisterType<UpdateProfile>();
  }
}
