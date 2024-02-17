using Effuse.SSO.Domain;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;
using AppDomain = Effuse.SSO.Domain;

namespace Effuse.SSO.Integration.Clients.User;

public class DbUserClient : IUserClient
{
  private struct UserServerDto
  {
    public string Url { get; set; }

    public string JoinedAt { get; set; }
  }

  private struct UserDto
  {
    public string UserName { get; set; }

    public string Email { get; set; }

    public string EncryptedPassword { get; set; }

    public string RegisteredAt { get; set; }

    public string LastSignIn { get; set; }

    public List<UserServerDto> Servers { get; set; }

    public string Biography { get; set; }
  }

  private struct UserEmailDto
  {
    public string UserId { get; set; }
  }

  private readonly IDatabase database;
  private readonly IStatic statics;

  private static string TableName => "Users";

  private static string EmailsName => "UserEmails";

  public DbUserClient(IDatabase database, IStatic statics)
  {
    this.database = database;
    this.statics = statics;
  }

  private static AppDomain.User FromDto(string userId, UserDto result)
  {
    return new AppDomain.User(
      userId: Guid.Parse(userId),
      email: result.Email,
      userName: result.UserName,
      encryptedPassword: result.EncryptedPassword,
      registeredAt: DateTime.Parse(result.RegisteredAt),
      lastSignIn: DateTime.Parse(result.LastSignIn),
      servers: result.Servers?.Select(s => new UserServer(s.Url, DateTime.Parse(s.JoinedAt))) ?? Array.Empty<UserServer>(),
      biography: result.Biography
    );
  }

  private static UserDto ToDto(AppDomain.User user)
  {
    return new UserDto
    {
      UserName = user.UserName,
      Biography = user.Biography,
      Email = user.Email,
      EncryptedPassword = user.EncryptedPassword,
      RegisteredAt = user.RegisteredAt.ToISOString(),
      LastSignIn = user.LastSignIn.ToISOString(),
      Servers = user.Servers.Select(s => new UserServerDto
      {
        Url = s.Url,
        JoinedAt = s.JoinedAt.ToISOString()
      }).ToList()
    };
  }

  private static string PictureName(AppDomain.User user)
  {
    return $"profile-pictures/{user.UserId.ToString()}";
  }

  public async Task CreateUser(AppDomain.User user)
  {
    await this.database.AddItem(TableName, user.UserId.ToString(), ToDto(user));
    await this.database.AddItem(EmailsName, user.Email, new UserEmailDto
    {
      UserId = user.UserId.ToString()
    });
  }

  public async Task DeleteUser(AppDomain.User user)
  {
    await this.database.DeleteItem(TableName, user.UserId.ToString());
    await this.database.DeleteItem(EmailsName, user.Email);
  }

  public async Task<AppDomain.User> GetUser(Guid userId)
  {
    var result = await this.database.GetItem<UserDto>(TableName, userId.ToString());

    return FromDto(userId.ToString(), result);
  }

  public async Task<AppDomain.User> GetUserByEmail(string email)
  {
    var userEmail = await this.database.GetItem<UserEmailDto>(EmailsName, email);
    var result = await this.database.GetItem<UserDto>(TableName, userEmail.UserId);

    return FromDto(userEmail.UserId, result);
  }

  public Task UpdateUser(AppDomain.User user)
  {
    return this.database.UpdateItem(TableName, user.UserId.ToString(), ToDto(user));
  }

  public async Task<MemoryStream> GetProfilePicture(AppDomain.User user)
  {
    var file = await this.statics.Download(PictureName(user));

    return file.Data;
  }

  public Task UploadProfilePicture(AppDomain.User user, MemoryStream imageData, string mime)
  {
    return this.statics.Upload(new StaticFile
    {
      Name = PictureName(user),
      Data = imageData,
      Mime = mime
    });
  }
}