using Effuse.Server.Handlers.Controllers;
using Effuse.Server.Handlers.Controllers.Admin;
using Unity;

namespace Effuse.Server.Handlers;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<Authenticate>();
    container.RegisterType<Chat>();
    container.RegisterType<Channels>();
    container.RegisterType<AddUserToChannel>();
    container.RegisterType<BanUser>();
    container.RegisterType<CreateChatChannel>();
    container.RegisterType<GiveUserAdmin>();
    container.RegisterType<KickUserFromChannel>();
    container.RegisterType<RenameChannel>();
  }
}
