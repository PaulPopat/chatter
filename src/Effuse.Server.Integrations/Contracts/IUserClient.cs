using Effuse.Server.Domain;

namespace Effuse.Server.Integrations.Contracts;

public interface IUserClient
{
  Task<User> GetUser(Guid userId);

  Task UpdateUser(User user);

  Task<User> RegisterUser(Guid userId);

  IAsyncEnumerable<User> ListUsers();
}
