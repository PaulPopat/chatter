using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;
using Effuse.Server.Domain;
using Effuse.Server.Integrations;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class AdminService(
  AuthService authService,
  IChannelClient channelClient,
  IUserClient userClient,
  IParameters parameters,
  IRoleClient roleClient)
{
  public async Task<Channel> CreateChatChannel(string localToken, string name, bool @public)
  {
    await authService.RequireAdmin(localToken);
    return await channelClient.CreateChannel(name, ChannelType.Messages, @public);
  }

  public async Task<Url> InviteLink(string localToken, string publicUrl, bool embedPassword, bool admin)
  {
    await authService.RequireAdmin(localToken);
    var query = new Dictionary<string, string>()
    {
      ["action"] = "join",
      ["server_url"] = publicUrl
    };

    var baseUrl = await parameters.GetParameter(ParameterName.UI_URL);
    if (embedPassword)
    {
      query["password"] = admin
      ? await parameters.GetParameter(ParameterName.SERVER_ADMIN_PASSWORD)
      : await parameters.GetParameter(ParameterName.SERVER_PASSWORD);
    }

    return new Url(baseUrl, "", query);
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

  public async Task SetUserRole(string localToken, Guid userId, Guid roleId)
  {
    await authService.RequireAdmin(localToken);
    var role = await roleClient.GetRole(roleId);
    var user = await userClient.GetUser(userId);

    await userClient.UpdateUser(user.WithRole(role));
  }

  public async Task<Role> CreateRole(string localToken, string name) {
    await authService.RequireAdmin(localToken);
    return await roleClient.AddRole(name);
  }

  public async Task<IEnumerable<Role>> GetAllRoles(string localToken)
  {
    await authService.RequireAdmin(localToken);
    return await roleClient.ListRole().ToListAsync();
  }

  public async Task AddRoleToChannel(string localToken, Guid channelId, Guid roleId, bool allowWrite)
  {
    await authService.RequireAdmin(localToken);
    var role = await roleClient.GetRole(roleId);
    var channel = await channelClient.GetChannel(channelId);
    await roleClient.UpdateRole(role.WithChannelAccess(
      channel,
      allowWrite ? UserPolicyAccess.Write : UserPolicyAccess.Read));
  }

  public async Task KickRoleFromChannel(string localToken, Guid channelId, Guid roleId)
  {
    await authService.RequireAdmin(localToken);
    var role = await roleClient.GetRole(roleId);
    var channel = await channelClient.GetChannel(channelId);
    await roleClient.UpdateRole(role.RevokeChannelAccess(channel));
  }

  public async Task GiveRoleAdmin(string localToken, Guid roleId)
  {
    await authService.RequireAdmin(localToken);
    var role = await roleClient.GetRole(roleId);
    await roleClient.UpdateRole(role.AsAdmin);
  }

  public async Task RevokeRoleAdmin(string localToken, Guid roleId)
  {
    await authService.RequireAdmin(localToken);
    var role = await roleClient.GetRole(roleId);
    await roleClient.UpdateRole(role.WithoutAdmin);
  }
}
