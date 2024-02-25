using Effuse.Core.Domain;

namespace Effuse.Server.Domain;

public class User
{
  public User(
    Guid userId,
    DateTime joinedServer,
    DateTime lastLoggedIn,
    bool banned,
    IEnumerable<UserPolicy> policies,
    bool admin)
  {
    UserId = userId;
    JoinedServer = joinedServer;
    LastLoggedIn = lastLoggedIn;
    Banned = banned;
    Policies = policies;
    Admin = admin;
  }

  public Guid UserId { get; }

  public DateTime JoinedServer { get; }

  public DateTime LastLoggedIn { get; }

  public bool Banned { get; }

  public IEnumerable<UserPolicy> Policies { get; }

  public bool Admin { get; }

  public bool MayRead(Channel channel)
  {
    if (this.Admin)
      return true;

    return this.Policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Read ?? false;
  }

  public bool MayWrite(Channel channel)
  {
    if (this.Admin)
      return true;

    return this.Policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Write ?? false;
  }

  public User WithChannelAccess(Channel channel, bool read, bool write)
  {
    return new User(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: Admin,
      policies: this.Policies.Append(new UserPolicy(
        channelId: channel.ChannelId,
        read: read,
        write: write)));
  }

  public User RevokeChannelAccess(Channel channel)
  {
    return new User(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: Admin,
      policies: this.Policies.Where(p => p.ChannelId != channel.ChannelId));
  }

  public User AsBanned => new(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: true,
      admin: Admin,
      policies: Policies);

  public User AsAdmin => new(
      userId: this.UserId,
      joinedServer: JoinedServer,
      lastLoggedIn: LastLoggedIn,
      banned: Banned,
      admin: true,
      policies: Policies);
}
