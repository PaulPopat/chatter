using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class Admin
{
  private readonly Auth authService;
  private readonly IChannelClient channelClient;
  private readonly IUserClient userClient;

  public Admin(Auth authService, IChannelClient channelClient, IUserClient userClient)
  {
    this.authService = authService;
    this.channelClient = channelClient;
    this.userClient = userClient;
  }

  public async Task<Channel> CreateChatChannel(string localToken, string name, bool @public)
  {
    await this.authService.RequireAdmin(localToken);
    return await this.channelClient.CreateChannel(name, ChannelType.Messages, @public);
  }

  public async Task<Channel> RenameChannel(string localToken, Guid channelId, string name)
  {
    await this.authService.RequireAdmin(localToken);
    var channel = await this.channelClient.GetChannel(channelId);
    return await this.channelClient.UpdateChannel(channel.WithName(name));
  }

  public async Task AddUserToChannel(string localToken, Guid channelId, Guid userId, bool allowWrite)
  {
    await this.authService.RequireAdmin(localToken);
    var user = await this.userClient.GetUser(userId);
    var channel = await this.channelClient.GetChannel(channelId);
    await this.userClient.UpdateUser(user.WithChannelAccess(channel, true, allowWrite));
  }

  public async Task KickUserFromChannel(string localToken, Guid channelId, Guid userId)
  {
    await this.authService.RequireAdmin(localToken);
    var user = await this.userClient.GetUser(userId);
    var channel = await this.channelClient.GetChannel(channelId);
    await this.userClient.UpdateUser(user.RevokeChannelAccess(channel));
  }

  public async Task BanUser(string localToken, Guid userId)
  {
    await this.authService.RequireAdmin(localToken);
    var user = await this.userClient.GetUser(userId);
    await this.userClient.UpdateUser(user.AsBanned);
  }

  public async Task GiveUserAdmin(string localToken, Guid userId)
  {
    await this.authService.RequireAdmin(localToken);
    var user = await this.userClient.GetUser(userId);
    await this.userClient.UpdateUser(user.AsAdmin);
  }
}
