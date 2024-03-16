using Effuse.Core.Domain;
using Effuse.Core.Integration;
using Effuse.Core.Integration.Contracts;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class MessagingService
{
  private readonly IChannelClient channelClient;
  private readonly AuthService auth;
  private readonly ISubscriptions subscriptions;
  private readonly IChatLog chatLog;
  private readonly IUserClient userClient;

  public MessagingService(IChannelClient channelClient, AuthService auth, ISubscriptions subscriptions, IChatLog chatLog, IUserClient userClient)
  {
    this.channelClient = channelClient;
    this.auth = auth;
    this.subscriptions = subscriptions;
    this.chatLog = chatLog;
    this.userClient = userClient;
  }

  public async Task PostMessage(string localToken, Guid channelId, string text)
  {
    var user = await this.auth.GetUser(localToken);
    var channel = await this.channelClient.GetChannel(channelId);
    if (!user.MayWrite(channel))
    {
      throw new AuthException("No write access");
    }

    var message = new Message(text, DateTime.Now, user.UserId);
    await this.chatLog.PostMessage(channel, message);

    await this.subscriptions.Broadcast(channel, message);
  }

  public async Task ListenToChannel(string localToken, Guid channelId, string connectionId)
  {
    var user = await this.auth.GetUser(localToken);
    var channel = await this.channelClient.GetChannel(channelId);
    if (!user.MayRead(channel))
    {
      throw new AuthException("No read access");
    }

    await this.subscriptions.Subscribe(new Subscription(
      subscriptionId: connectionId,
      userId: user.UserId,
      channelId: channel.ChannelId));
  }

  public async Task<IEnumerable<Message>> GetBackLog(string localToken, Guid channelId, long offset)
  {
    var user = await this.auth.GetUser(localToken);
    var channel = await this.channelClient.GetChannel(channelId);
    if (!user.MayRead(channel))
    {
      throw new AuthException("No read access");
    }

    return await this.chatLog.GetMessageLogs(channel, offset).ToListAsync();
  }

  public async Task Disconnect(string connectionId)
  {
    await this.subscriptions.Unsubscribe(connectionId);
  }
}
