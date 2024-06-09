using Effuse.Core.Domain;

namespace Effuse.Server.Domain;

public class Role(Guid roleId, string name, IEnumerable<UserPolicy> policies, bool admin)
{
  public Guid RoleId => roleId;

  public string Name => name;

  public bool Admin => admin;

  public IEnumerable<UserPolicy> Policies => policies;

  public bool MayRead(Channel channel) =>
    admin || (policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Read ?? false);

  public bool MayWrite(Channel channel) =>
    admin || (policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Write ?? false);

  public static Role Empty => new(Guid.Empty, string.Empty, [], false);
}
