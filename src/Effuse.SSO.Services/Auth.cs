using System.Security.Cryptography;
using Effuse.Core.Integration.Contracts;
using Effuse.SSO.Domain;
using Effuse.SSO.Integration.Clients.User;

namespace Effuse.SSO.Services;

public class UserGrant(string userToken, string serverToken, string refreshToken, Guid userId, DateTime expires)
{
  public string UserToken { get; } = userToken;

  public string ServerToken { get; } = serverToken;

  public string RefreshToken { get; } = refreshToken;

  public Guid UserId { get; } = userId;

  public DateTime Expires { get; } = expires;
}

public enum UserAccess
{
  Admin = 0,
  Identify = 1,
}

public class AuthService(IUserClient userClient, IJwtClient jwtClient)
{
  private struct UserTokenPayload
  {
    public string UserId { get; set; }

    public UserAccess Access { get; set; }
  }

  private struct UserRefreshPayload
  {
    public string UserId { get; set; }
  }


  private readonly IUserClient userClient = userClient;
  private readonly IJwtClient jwtClient = jwtClient;

  private string EncryptPassowrd(string input)
  {
    var salt = RandomNumberGenerator.GetBytes(16);
    var hash = Rfc2898DeriveBytes.Pbkdf2(
        input,
        salt,
        50000,
        HashAlgorithmName.SHA256,
        32
    );
    return string.Join(
        ':',
        Convert.ToHexString(hash),
        Convert.ToHexString(salt),
        50000,
        HashAlgorithmName.SHA256
    );
  }

  private bool VerifyPassword(string input, string hashString)
  {
    var segments = hashString.Split(':');
    var hash = Convert.FromHexString(segments[0]);
    var salt = Convert.FromHexString(segments[1]);
    var iterations = int.Parse(segments[2]);
    var algorithm = new HashAlgorithmName(segments[3]);
    byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
        input,
        salt,
        iterations,
        algorithm,
        hash.Length
    );
    return CryptographicOperations.FixedTimeEquals(inputHash, hash);
  }

  private async Task<UserGrant> CreateGrant(User user)
  {
    return new UserGrant(
      await this.jwtClient.CreateJwt(
        new UserTokenPayload
        {
          UserId = user.UserId.ToString(),
          Access = UserAccess.Admin
        },
        TimeSpan.FromHours(12)),
      await this.jwtClient.CreateJwt(
        new UserTokenPayload
        {
          UserId = user.UserId.ToString(),
          Access = UserAccess.Identify
        },
        TimeSpan.FromHours(12)),
        await this.jwtClient.CreateJwt(
        new UserRefreshPayload
        {
          UserId = user.UserId.ToString()
        },
        TimeSpan.FromDays(7)),
        user.UserId,
        DateTime.Now + TimeSpan.FromHours(12));
  }

  public async Task<UserGrant> Register(string username, string email, string password)
  {
    var user = new User(
      userId: Guid.NewGuid(),
      userName: username,
      email: email,
      encryptedPassword: this.EncryptPassowrd(password),
      registeredAt: DateTime.Now,
      lastSignIn: DateTime.Now,
      servers: new List<UserServer>(),
      biography: string.Empty);

    await this.userClient.CreateUser(user);

    return await this.CreateGrant(user);
  }

  public async Task<UserGrant> Login(string email, string password)
  {
    var user = await this.userClient.GetUserByEmail(email);

    if (!this.VerifyPassword(password, user.EncryptedPassword))
      throw new Exception("Access denied");

    await this.userClient.UpdateUser(user.LoggedIn());

    return await this.CreateGrant(user);
  }

  public async Task<UserGrant> Refresh(string refreshToken)
  {
    var grant = await this.jwtClient.DecodeJwt<UserRefreshPayload>(refreshToken);

    var user = await this.userClient.GetUser(Guid.Parse(grant.UserId));
    return await this.CreateGrant(user);
  }

  public async Task<Guid> Verify(string token, UserAccess access)
  {
    var grant = await this.jwtClient.DecodeJwt<UserTokenPayload>(token);

    if (grant.Access != access) throw new Exception("Invalid token");
    return Guid.Parse(grant.UserId);
  }
}