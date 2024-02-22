using Effuse.Server.Domain;

namespace Effuse.Server.Integrations.Contracts;

public interface IUserClient
{
  Task<User> GetUser(Guid userId);

  Task<User?> FindUser(Guid userId);

  Task UpdateUser(User user);

  Task<User> RegisterUser(Guid userId, bool admin);

  IAsyncEnumerable<User> ListUsers();
}
