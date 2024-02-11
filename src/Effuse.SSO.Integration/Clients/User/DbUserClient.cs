using Effuse.SSO.Domain;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;
using AppDomain = Effuse.SSO.Domain;

namespace Effuse.SSO.Integration.Clients.User;

public class DbUserClient : IUserClient
{
  private class UserServerDto
  {
    public string Url { get; set; } = "";

    public string JoinedAt { get; set; } = "";
  }

  private struct UserDto
  {
    public string UserId { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string EncryptedPassword { get; set; }

    public string RegisteredAt { get; set; }

    public string LastSignIn { get; set; }

    public List<UserServerDto> Servers { get; set; }

    public string Biography { get; set; }
  }

  private readonly IDatabase database;
  private readonly IStatic statics;

  private static string TableName => Env.GetEnv("USER_TABLE_NAME");

  private static string EmailIndexName => Env.GetEnv("USER_TABLE_EMAIL_INDEX");

  public DbUserClient(IDatabase database, IStatic statics)
  {
    this.database = database;
    this.statics = statics;
  }

  private AppDomain.User FromDto(UserDto result)
  {
    return new AppDomain.User(
      userId: Guid.Parse(result.UserId),
      email: result.Email,
      userName: result.UserName,
      encryptedPassword: result.EncryptedPassword,
      registeredAt: DateTime.Parse(result.RegisteredAt),
      lastSignIn: DateTime.Parse(result.LastSignIn),
      servers: result.Servers?.Select(s => new UserServer(s.Url, DateTime.Parse(s.JoinedAt))) ?? Array.Empty<UserServer>(),
      biography: result.Biography
    );
  }

  private UserDto ToDto(AppDomain.User user)
  {
    return new UserDto
    {
      UserId = user.UserId.ToString(),
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

  private string PictureName(AppDomain.User user)
  {
    return $"profile-pictures/{user.UserId.ToString()}";
  }

  public Task CreateUser(AppDomain.User user)
  {
    return this.database.AddItem(TableName, this.ToDto(user));
  }

  public Task DeleteUser(AppDomain.User user)
  {
    return this.database.DeleteItem(new DeleteItemCommand
    {
      TableName = TableName,
      KeyName = "UserId",
      KeyValue = user.UserId.ToString()
    });
  }

  public async Task<AppDomain.User> GetUser(Guid userId)
  {
    var result = await this.database.GetItem<UserDto>(new GetItemCommand
    {
      TableName = TableName,
      KeyName = "UserId",
      KeyValue = userId.ToString()
    });

    return this.FromDto(result);
  }

  public async Task<AppDomain.User> GetUserByEmail(string email)
  {
    var results = await this.database.Query<UserDto>(new QueryCommand
    {
      TableName = TableName,
      KeyName = "Email",
      KeyValue = email,
      IndexName = EmailIndexName
    });

    var resultList = results.ToList();
    if (resultList.Count != 1) throw new Exception("Could not find user via email");

    var result = resultList[0];
    return this.FromDto(result);
  }

  public Task UpdateUser(AppDomain.User user)
  {
    return this.database.UpdateItem(TableName, this.ToDto(user));
  }

  public async Task<MemoryStream> GetProfilePicture(AppDomain.User user)
  {
    var file = await this.statics.Download(this.PictureName(user));

    return file.Data;
  }

  public Task UploadProfilePicture(AppDomain.User user, MemoryStream imageData, string mime)
  {
    return this.statics.Upload(new StaticFile
    {
      Name = this.PictureName(user),
      Data = imageData,
      Mime = mime
    });
  }
}