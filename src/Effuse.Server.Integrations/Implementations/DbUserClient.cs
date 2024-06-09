using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;
using Effuse.Server.Domain;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Integrations;

public class DbUserClient(IDatabase database, IChannelClient channelClient, IRoleClient roleClient) : IUserClient
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

    public string RoleId { get; set; }
  }

  private struct ListUsersDto
  {
    public List<string> Users { get; set; }
  }

  private static string TableName => "ServerUsers";
  private static string ListTableName => "ListServerUsers";
  private static string ListItemName => "ListServerUsersItem";

  private static UserDto ToDto(User user)
  {
    return new UserDto
    {
      JoinedServer = user.JoinedServer.ToISOString(),
      LastLoggedIn = user.LastLoggedIn.ToISOString(),
      Banned = user.Banned,
      Admin = user.Admin,
      RoleId = user.Role.RoleId.ToString(),
      Policies = user.Policies.Select(p => new UserPolicyDto
      {
        ChannelId = p.ChannelId.ToString(),
        Read = p.Read,
        Write = p.Write
      }).ToList()
    };
  }

  private static User FromDto(Guid userId, UserDto dto, Role role)
  {
    return new User(
      userId,
      DateTime.Parse(dto.JoinedServer),
      DateTime.Parse(dto.LastLoggedIn),
      dto.Banned,
      role,
      dto.Policies.Select(p => new UserPolicy(
        channelId: Guid.Parse(p.ChannelId),
        access: p.Write ? UserPolicyAccess.Write : UserPolicyAccess.Read)),
      dto.Admin);
  }

  public async Task UpdateUser(User user)
  {
    await database.UpdateItem(TableName, user.UserId.ToString(), ToDto(user));
  }

  public async Task<User> GetUser(Guid userId)
  {
    var userRow = await database.GetItem<UserDto>(TableName, userId.ToString());
    var roleId = Guid.Parse(userRow.RoleId);
    var role = roleId != Guid.Empty ? await roleClient.GetRole(roleId) : Role.Empty;
    return FromDto(userId, userRow, role);
  }

  public async Task<User> RegisterUser(Guid userId, bool admin)
  {
    var input = new User(
      userId: userId,
      joinedServer: DateTime.Now,
      lastLoggedIn: DateTime.Now,
      banned: false,
      role: Role.Empty,
      policies: await channelClient.ListChannels()
        .Where(c => c.Public)
        .Select(c => new UserPolicy(
          channelId: c.ChannelId,
          access: UserPolicyAccess.Write)).ToListAsync(),
      admin: admin);

    await database.AddItem(TableName, userId.ToString(), ToDto(input));

    var existing = await database.FindItem<ListUsersDto>(ListTableName, ListItemName);
    if (existing == null)
    {
      await database.AddItem(ListTableName, ListItemName, new ListUsersDto
      {
        Users = [input.UserId.ToString()]
      });
    }
    else
    {
      await database.UpdateItem(ListTableName, ListItemName, new ListUsersDto
      {
        Users = existing.Value.Users.Append(input.UserId.ToString()).ToList()
      });
    }

    return input;
  }

  public async IAsyncEnumerable<User> ListUsers()
  {
    var listing = await database.FindItem<ListUsersDto>(ListTableName, ListItemName);

    if (listing == null) yield break;

    foreach (var userId in listing.Value.Users)
    {
      var user = await this.GetUser(Guid.Parse(userId));
      if (user.Banned) continue;
      yield return user;
    }
  }

  public async IAsyncEnumerable<User> ListBannedUsers()
  {
    var listing = await database.FindItem<ListUsersDto>(ListTableName, ListItemName);

    if (listing == null) yield break;

    foreach (var userId in listing.Value.Users)
    {
      var user = await this.GetUser(Guid.Parse(userId));
      if (!user.Banned) continue;
      yield return user;
    }
  }

  public async Task<User?> FindUser(Guid userId)
  {
    var dto = await database.FindItem<UserDto>(TableName, userId.ToString());
    if (dto == null) return null;
    var roleId = Guid.Parse(dto.Value.RoleId);
    var role = roleId != Guid.Empty ? await roleClient.GetRole(roleId) : Role.Empty;
    return FromDto(userId, dto.Value, role);
  }
}
