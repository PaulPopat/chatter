using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;
using Effuse.Server.Domain;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Integrations;

public class DbUserClient : IUserClient
{
  private struct ServerTokenDto
  {
    public string UserId { get; set; }
  }

  private struct UserPolicyDto
  {
    public string ChannelId { get; set; }

    public bool Read { get; set; }

    public bool Write { get; set; }
  }

  private struct UserDto
  {
    public string JoinedServer { get; set; }

    public string LastLoggedIn { get; set; }

    public bool Banned { get; set; }

    public bool Admin { get; set; }

    public List<UserPolicyDto> Policies { get; set; }
  }

  private struct ListUsersDto
  {
    public List<string> Users { get; set; }
  }

  private static string TableName => "ServerUsers";
  private static string ListTableName => "ListServerUsers";
  private static string ListItemName => "ListServerUsersItem";

  private readonly IDatabase database;
  private readonly IChannelClient channelClient;

  public DbUserClient(IDatabase database, IChannelClient channelClient)
  {
    this.database = database;
    this.channelClient = channelClient;
  }

  private static UserDto ToDto(User user)
  {
    return new UserDto
    {
      JoinedServer = user.JoinedServer.ToISOString(),
      LastLoggedIn = user.LastLoggedIn.ToISOString(),
      Banned = user.Banned,
      Admin = user.Admin,
      Policies = user.Policies.Select(p => new UserPolicyDto
      {
        ChannelId = p.ChannelId.ToString(),
        Read = p.Read,
        Write = p.Write
      }).ToList()
    };
  }

  private static User FromDto(Guid userId, UserDto dto)
  {
    return new User(
      userId,
      DateTime.Parse(dto.JoinedServer),
      DateTime.Parse(dto.LastLoggedIn),
      dto.Banned,
      dto.Policies.Select(p => new UserPolicy(
        channelId: Guid.Parse(p.ChannelId),
        read: p.Read,
        write: p.Write)),
      dto.Admin);
  }

  public async Task UpdateUser(User user)
  {
    await this.database.UpdateItem(TableName, user.UserId.ToString(), ToDto(user));
  }

  public async Task<User> GetUser(Guid userId)
  {
    return FromDto(userId, await this.database.GetItem<UserDto>(TableName, userId.ToString())); ;
  }

  public async Task<User> RegisterUser(Guid userId)
  {
    var input = new User(
      userId,
      DateTime.Now,
      DateTime.Now,
      false,
      await this.channelClient.ListChannels()
        .Where(c => c.Public)
        .Select(c => new UserPolicy(
          channelId: c.ChannelId,
          read: true,
          write: true)).ToListAsync(),
      false);

    await this.database.AddItem(TableName, userId.ToString(), ToDto(input));

    var existing = await this.database.FindItem<ListUsersDto>(ListTableName, ListItemName);
    if (existing == null)
    {
      await this.database.AddItem(ListTableName, ListItemName, new ListUsersDto
      {
        Users = new List<string>() { input.UserId.ToString() }
      });
    }
    else
    {
      await this.database.UpdateItem(ListTableName, ListItemName, new ListUsersDto
      {
        Users = existing.Value.Users.Append(input.UserId.ToString()).ToList()
      });
    }

    return input;
  }

  public async IAsyncEnumerable<User> ListUsers()
  {
    var listing = await this.database.FindItem<ListUsersDto>(ListTableName, ListItemName);

    if (listing == null) yield break;

    foreach (var userId in listing.Value.Users)
    {
      yield return await this.GetUser(Guid.Parse(userId));
    }
  }

  public async Task<User?> FindUser(Guid userId)
  {
    var dto = await this.database.FindItem<UserDto>(TableName, userId.ToString());
    if (dto == null) return null;
    return FromDto(userId, dto.Value);
  }
}
