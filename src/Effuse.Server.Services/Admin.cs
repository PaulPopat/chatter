using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;
using Effuse.Server.Domain;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class AdminService(AuthService authService, IChannelClient channelClient, IUserClient userClient)
{
  public async Task<Channel> CreateChatChannel(string localToken, string name, bool @public)
  {
    await authService.RequireAdmin(localToken);
    return await channelClient.CreateChannel(name, ChannelType.Messages, @public);
  }

  public async Task<Channel> UpdateChannel(string localToken, Guid channelId, string name, bool @public)
  {
    await authService.RequireAdmin(localToken);
    var channel = await channelClient.GetChannel(channelId);
    return await channelClient.UpdateChannel(channel.WithName(name).WithPublicity(@public));
  }

  public async Task AddUserToChannel(string localToken, Guid channelId, Guid userId, bool allowWrite)
  {
    await authService.RequireAdmin(localToken);
    var user = await userClient.GetUser(userId);
    var channel = await channelClient.GetChannel(channelId);
    await userClient.UpdateUser(user.WithChannelAccess(
      channel,
      allowWrite ? UserPolicyAccess.Write : UserPolicyAccess.Read));
  }

  public async Task KickUserFromChannel(string localToken, Guid channelId, Guid userId)
  {
    await authService.RequireAdmin(localToken);
    var user = await userClient.GetUser(userId);
    var channel = await channelClient.GetChannel(channelId);
    await userClient.UpdateUser(user.RevokeChannelAccess(channel));
  }

  public async Task BanUser(string localToken, Guid userId)
  {
    await authService.RequireAdmin(localToken);
    var user = await userClient.GetUser(userId);
    await userClient.UpdateUser(user.AsBanned);
  }

  public async Task GiveUserAdmin(string localToken, Guid userId)
  {
    await authService.RequireAdmin(localToken);
    var user = await userClient.GetUser(userId);
    await userClient.UpdateUser(user.AsAdmin);
  }

  public async Task RevokeUserAdmin(string localToken, Guid userId)
  {
    await authService.RequireAdmin(localToken);
    var user = await userClient.GetUser(userId);
    await userClient.UpdateUser(user.WithoutAdmin);
  }

  public async Task<IEnumerable<User>> GetAllUsers(string localToken)
  {
    await authService.RequireAdmin(localToken);
    return await userClient.ListUsers().ToListAsync();
  }

  public async Task<IEnumerable<User>> GetAllBannedUsers(string localToken)
  {
    await authService.RequireAdmin(localToken);
    return await userClient.ListBannedUsers().ToListAsync();
  }


  public async Task<IEnumerable<UserPolicy>> GetUserPolicies(string localToken, Guid userId)
  {
    await authService.RequireAdmin(localToken);
    var user = await userClient.GetUser(userId);
    return user.Policies;
  }
}
