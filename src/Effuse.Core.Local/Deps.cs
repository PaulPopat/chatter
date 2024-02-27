using Effuse.Core.Integration.Contracts;
using Effuse.Core.Local.Integrations;
using Unity;

namespace Effuse.Core.Local;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<IStatic, DiskStatics>();
    container.RegisterType<IParameters, SystemParameters>();
    container.RegisterType<ISubscriptions, WebsocketSubscriptions>();
  }
}
