using System.Security.Cryptography;
using System.Text.Json;
using Effuse.SSO.Domain;
using Effuse.SSO.Integration.Clients.Jwt;
using Effuse.SSO.Integration.Clients.User;

namespace Effuse.SSO.Services;

public class UserGrant
{
  public UserGrant(string userToken, string serverToken)
  {
    UserToken = userToken;
    ServerToken = serverToken;
  }

  public string UserToken { get; }

  public string ServerToken { get; }
}

public enum UserAccess
{
  Admin = 0,
  Readonly = 1,
  Identify = 2,
}

public class AuthService
{

  private class UserToken
  {
    public string UserId { get; set; } = "";

    public UserAccess Access { get; set; }
  }


  private readonly IUserClient userClient;
  private readonly IJwtClient jwtClient;

  public AuthService(IUserClient userClient, IJwtClient jwtClient)
  {
    this.userClient = userClient;
    this.jwtClient = jwtClient;
  }

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
        JsonSerializer.Serialize(new UserToken
        {
          UserId = user.UserId.ToString(),
          Access = UserAccess.Admin
        }),
        DateTime.Now.AddHours(12)),
      await this.jwtClient.CreateJwt(
        JsonSerializer.Serialize(new UserToken
        {
          UserId = user.UserId.ToString(),
          Access = UserAccess.Identify
        }),
        DateTime.Now.AddHours(1)));
  }

  public async Task<UserGrant> Register(string email, string password, string invite)
  {
    var invitedEmail = await this.jwtClient.DecodeJwt(invite);

    if (invitedEmail != email)
      throw new Exception("User not invited");

    var user = new User(
      new Guid(),
      email,
      this.EncryptPassowrd(password),
      DateTime.Now,
      DateTime.Now,
      new List<UserServer>());

    await this.userClient.CreateUser(user);

    return await this.CreateGrant(user);
  }

  public async Task<UserGrant> Login(string email, string password)
  {
    var user = await this.userClient.GetUserByEmail(email);

    if (!this.VerifyPassword(password, user.EncryptedPassword))
      throw new Exception("Access denied");

    return await this.CreateGrant(user);
  }

  public async Task<string> Verify(string token, UserAccess access)
  {
    var json = await this.jwtClient.DecodeJwt(token);
    var grant = JsonSerializer.Deserialize<UserToken>(json);

    if (grant?.Access != access) throw new Exception("Invalid token");

    return grant.UserId;
  }

  public async Task<string> CreateInvite(string email)
  {
    return await this.jwtClient.CreateJwt(email, DateTime.Now.AddDays(7));
  }
}