using Effuse.Core.Integration.Contracts;
using Effuse.Core.Integration.Implementations;
using Unity;

namespace Effuse.Core.Integration;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<IDatabase, StaticDatabase>();
    container.RegisterType<IEncryption, Encryption>();
    container.RegisterType<IJwtClient, ParameterJwtClient>();
    container.RegisterType<IChatLog, ChatLog>();
    container.RegisterType<IChannelClient, DbChannelClient>();
  }
}
