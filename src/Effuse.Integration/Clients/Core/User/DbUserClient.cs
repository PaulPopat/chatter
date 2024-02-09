using Effuse.Domain.Core;
using Effuse.Integration.Contracts;
using Effuse.Integration.Utilities;
using AppDomain = Effuse.Domain.Core;

namespace Effuse.Integration.Clients.User;

public class DbUserClient : IUserClient
{
  private class UserServerDto
  {
    public string Url { get; set; } = "";

    public string JoinedAt { get; set; } = "";
  }

  private class UserDto
  {
    public string UserId { get; set; } = "";

    public string Email { get; set; } = "";

    public string EncryptedPassword { get; set; } = "";

    public string RegisteredAt { get; set; } = "";

    public string LastSignIn { get; set; } = "";

    public List<UserServerDto>? Servers { get; set; }
  }

  private readonly IDatabase database;

  private static string TableName => Environment.GetEnvironmentVariable("USER_TABLE_NAME") ?? throw new Exception("Could not find user table name");

  private static string EmailIndexName => Environment.GetEnvironmentVariable("USER_TABLE_EMAIL_INDEX") ?? throw new Exception("Could not find user table name");


  public DbUserClient(IDatabase database)
  {
    this.database = database;
  }

  public Task CreateUser(AppDomain.User user)
  {
    return this.database.AddItem(TableName, new UserDto
    {
      UserId = user.UserId.ToString(),
      Email = user.Email,
      EncryptedPassword = user.EncryptedPassword,
      RegisteredAt = user.RegisteredAt.ToISOString(),
      LastSignIn = user.LastSignIn.ToISOString(),
      Servers = user.Servers.Select(s => new UserServerDto
      {
        Url = s.Url,
        JoinedAt = s.JoinedAt.ToISOString()
      }).ToList()
    });
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

    return new AppDomain.User(
      Guid.Parse(result.UserId),
      result.Email,
      result.EncryptedPassword,
      DateTime.Parse(result.RegisteredAt),
      DateTime.Parse(result.LastSignIn),
      result.Servers?.Select(s => new AppDomain.UserServer(s.Url, DateTime.Parse(s.JoinedAt))) ?? Array.Empty<UserServer>()
    );
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

    return new AppDomain.User(
      Guid.Parse(result.UserId),
      result.Email,
      result.EncryptedPassword,
      DateTime.Parse(result.RegisteredAt),
      DateTime.Parse(result.LastSignIn),
      result.Servers?.Select(s => new AppDomain.UserServer(s.Url, DateTime.Parse(s.JoinedAt))) ?? Array.Empty<UserServer>()
    );
  }

  public Task UpdateUser(AppDomain.User user)
  {
    return this.database.UpdateItem(TableName, new UserDto
    {
      UserId = user.UserId.ToString(),
      Email = user.Email,
      EncryptedPassword = user.EncryptedPassword,
      RegisteredAt = user.RegisteredAt.ToISOString(),
      LastSignIn = user.LastSignIn.ToISOString(),
      Servers = user.Servers.Select(s => new UserServerDto
      {
        Url = s.Url,
        JoinedAt = s.JoinedAt.ToISOString()
      }).ToList()
    });
  }
}