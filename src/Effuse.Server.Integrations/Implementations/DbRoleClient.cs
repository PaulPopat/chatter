using Effuse.Core.Integration.Contracts;
using Effuse.Server.Domain;

namespace Effuse.Server.Integrations;

public class DbRoleClient(IDatabase database) : IRoleClient
{
  private struct UserPolicyDto
  {
    public string ChannelId { get; set; }

    public bool Read { get; set; }

    public bool Write { get; set; }
  }

  private struct RoleDto
  {
    public string Name { get; set; }

    public bool Admin { get; set; }

    public List<UserPolicyDto> Policies { get; set; }
  }

  private struct ListRolesDto
  {
    public List<string> Roles { get; set; }
  }


  private static string TableName => "ServerRoles";
  private static string ListTableName => "ListServerRoles";
  private static string ListItemName => "ListServerRolesItem";

  private static RoleDto ToDto(Role role) => new()
  {
    Name = role.Name,
    Admin = role.Admin,
    Policies = role.Policies.Select(p => new UserPolicyDto
    {
      ChannelId = p.ChannelId.ToString(),
      Read = p.Read,
      Write = p.Write
    }).ToList()
  };

  private static Role FromDto(Guid roleId, RoleDto dto) => new(
    roleId: roleId,
    name: dto.Name,
    policies: dto.Policies.Select(p => new UserPolicy(
      channelId: Guid.Parse(p.ChannelId),
      access: p.Write ? UserPolicyAccess.Write : UserPolicyAccess.Read)),
    admin: dto.Admin);

  public async Task<Role?> FindRole(Guid roleId)
  {
    var dto = await database.FindItem<RoleDto>(TableName, roleId.ToString());
    if (dto == null) return null;
    return FromDto(roleId, dto.Value);
  }

  public async Task<Role> GetRole(Guid roleId)
  {
    var roleRow = await database.GetItem<RoleDto>(TableName, roleId.ToString());
    return FromDto(roleId, roleRow);
  }

  public async IAsyncEnumerable<Role> ListRole()
  {
    var listing = await database.FindItem<ListRolesDto>(ListTableName, ListItemName);

    if (listing == null) yield break;

    foreach (var userId in listing.Value.Roles)
    {
      var role = await GetRole(Guid.Parse(userId));
      yield return role;
    }
  }

  public async Task<Role> AddRole(string name)
  {
    var input = new Role(
      roleId: Guid.NewGuid(),
      name: name,
      policies: [],
      admin: false);

    await database.AddItem(TableName, input.RoleId.ToString(), ToDto(input));

    var existing = await database.FindItem<ListRolesDto>(ListTableName, ListItemName);
    if (existing == null)
    {
      await database.AddItem(ListTableName, ListItemName, new ListRolesDto
      {
        Roles = [input.RoleId.ToString()]
      });
    }
    else
    {
      await database.UpdateItem(ListTableName, ListItemName, new ListRolesDto
      {
        Roles = existing.Value.Roles.Append(input.RoleId.ToString()).ToList()
      });
    }

    return input;
  }

  public async Task UpdateRole(Role role)
  {
    await database.UpdateItem(TableName, role.RoleId.ToString(), ToDto(role));
  }
}
