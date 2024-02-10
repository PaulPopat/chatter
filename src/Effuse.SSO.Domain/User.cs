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
      string userName,
      string encryptedPassword,
      DateTime registeredAt,
      DateTime lastSignIn,
      IEnumerable<UserServer> servers,
      string biography
    )
  {
    this.UserId = userId;
    this.Email = email;
    this.UserName = userName;
    this.EncryptedPassword = encryptedPassword;
    this.RegisteredAt = registeredAt;
    this.LastSignIn = lastSignIn;
    this.Servers = servers;
    this.Biography = biography;
  }

  public Guid UserId { get; }

  public string Email { get; }

  public string EncryptedPassword { get; }

  public DateTime RegisteredAt { get; }

  public DateTime LastSignIn { get; }

  public IEnumerable<UserServer> Servers { get; }

  public string UserName { get; }

  public string Biography { get; }

  public User WithUserName(string username)
  {
    return new User(
      this.UserId,
      this.Email,
      username,
      this.EncryptedPassword,
      this.RegisteredAt,
      this.LastSignIn,
      this.Servers,
      this.Biography);
  }

  public User WithBiography(string biography)
  {
    return new User(
      this.UserId,
      this.Email,
      this.UserName,
      this.EncryptedPassword,
      this.RegisteredAt,
      this.LastSignIn,
      this.Servers,
      biography);
  }

  public User LoggedIn()
  {
    return new User(
      this.UserId,
      this.Email,
      this.UserName,
      this.EncryptedPassword,
      this.RegisteredAt,
      DateTime.Now,
      this.Servers,
      this.Biography);
  }

  public User WithServer(string serverUrl)
  {
    return new User(
      this.UserId,
      this.Email,
      this.UserName,
      this.EncryptedPassword,
      this.RegisteredAt,
      this.LastSignIn,
      this.Servers.Append(new UserServer(serverUrl, DateTime.Now)),
      this.Biography
    );
  }
}