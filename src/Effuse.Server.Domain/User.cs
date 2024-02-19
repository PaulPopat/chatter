using Effuse.Core.Domain;

namespace Effuse.Server.Domain;

public class User
{
  public User(
    Guid userId,
    DateTime joinedServer,
    DateTime lastLoggedIn,
    bool banned,
    IEnumerable<UserPolicy> policies)
  {
    UserId = userId;
    JoinedServer = joinedServer;
    LastLoggedIn = lastLoggedIn;
    Banned = banned;
    Policies = policies;
  }

  public Guid UserId { get; }

  public DateTime JoinedServer { get; }

  public DateTime LastLoggedIn { get; }

  public bool Banned { get; }

  public IEnumerable<UserPolicy> Policies { get; }

  public bool MayRead(Channel channel)
  {
    return this.Policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Read ?? false;
  }

  public bool MayWrite(Channel channel)
  {
    return this.Policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Write ?? false;
  }

  public bool MayAdmin(Channel channel)
  {
    return this.Policies.FirstOrDefault(p => p.ChannelId == channel.ChannelId)?.Admin ?? false;
  }

  public User AsBanned => new(UserId, JoinedServer, LastLoggedIn, true, Policies);
}
