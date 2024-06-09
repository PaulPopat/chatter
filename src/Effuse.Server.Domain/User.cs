using System.Diagnostics.CodeAnalysis;
using Effuse.Core.Domain;

namespace Effuse.Server.Domain;

public class User(
    Guid userId,
    DateTime joinedServer,
    DateTime lastLoggedIn,
    bool banned,
    [AllowNull] Role? role,
    IEnumerable<UserPolicy> policies,
    bool admin)
{
  public Guid UserId => userId;

  public DateTime JoinedServer => joinedServer;

  public DateTime LastLoggedIn => lastLoggedIn;

  public bool Banned => banned;

  public Role Role => role ?? Role.Empty;

  public IEnumerable<UserPolicy> Policies => policies;

  public bool Admin => admin || Role.Admin;

  public bool MayRead(Channel channel) =>
    Role.MayRead(channel) ||
    admin ||
    (policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Read ?? false);

  public bool MayWrite(Channel channel) =>
    Role.MayWrite(channel) ||
    admin ||
    (policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Write ?? false);

  public User WithChannelAccess(Channel channel, UserPolicyAccess access)
  {
    return new User(
      userId: UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: Admin,
      policies: this.Policies.Append(new UserPolicy(
        channelId: channel.ChannelId,
        access: access)),
      role: role);
  }

  public User RevokeChannelAccess(Channel channel)
  {
    return new User(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: Admin,
      policies: this.Policies.Where(p => p.ChannelId != channel.ChannelId),
      role: role);
  }

  public User WithRole(Role role) => new(
      userId: UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: Admin,
      policies: Policies,
      role: role);

  public User AsBanned => new(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: true,
      admin: Admin,
      policies: Policies,
      role: role);

  public User AsAdmin => new(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: true,
      policies: Policies,
      role: role);

  public User WithoutAdmin => new(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: false,
      policies: Policies,
      role: role);
}
