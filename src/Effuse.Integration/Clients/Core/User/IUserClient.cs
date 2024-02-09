using AppDomain = Effuse.Domain.Core;

namespace Effuse.Integration.Clients.User;

public interface IUserClient
{
  Task<AppDomain.User> GetUser(Guid userId);

  Task<AppDomain.User> GetUserByEmail(string email);

  Task CreateUser(AppDomain.User user);

  Task UpdateUser(AppDomain.User user);

  Task DeleteUser(AppDomain.User user);
}