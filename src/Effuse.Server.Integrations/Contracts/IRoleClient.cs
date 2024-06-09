using Effuse.Server.Domain;

namespace Effuse.Server.Integrations;

public interface IRoleClient
{
  Task<Role> GetRole(Guid roleId);

  Task<Role?> FindRole(Guid roleId);

  Task<Role> AddRole(string name);

  Task UpdateRole(Role role);

  IAsyncEnumerable<Role> ListRole();
}
