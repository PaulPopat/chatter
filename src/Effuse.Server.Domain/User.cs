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
}
