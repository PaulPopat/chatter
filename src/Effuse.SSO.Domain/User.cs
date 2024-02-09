namespace Effuse.SSO.Domain;

public class UserServer
{
  public UserServer
    (
      string url,
      DateTime joinedAt
    )
  {
    this.Url = url;
    this.JoinedAt = joinedAt;
  }

  public string Url { get; }

  public DateTime JoinedAt { get; }
}

public class User
{
  public User
    (
      Guid userId,
      string email,
      string encryptedPassword,
      DateTime registeredAt,
      DateTime lastSignIn,
      IEnumerable<UserServer> servers
    )
  {
    this.UserId = userId;
    this.Email = email;
    this.EncryptedPassword = encryptedPassword;
    this.RegisteredAt = registeredAt;
    this.LastSignIn = lastSignIn;
    this.Servers = servers;
  }

  public Guid UserId { get; }

  public string Email { get; }

  public string EncryptedPassword { get; }

  public DateTime RegisteredAt { get; }

  public DateTime LastSignIn { get; }

  public IEnumerable<UserServer> Servers { get; }
}