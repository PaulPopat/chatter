using Effuse.Core.Domain;
using Effuse.Core.Integration;
using Effuse.Core.Integration.Contracts;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class Messaging
{
  private readonly IChannelClient channelClient;
  private readonly Auth auth;
  private readonly ISubscriptions subscriptions;
  private readonly IChatLog chatLog;
  private readonly IUserClient userClient;

  public Messaging(IChannelClient channelClient, Auth auth, ISubscriptions subscriptions, IChatLog chatLog, IUserClient userClient)
  {
    this.channelClient = channelClient;
    this.auth = auth;
    this.subscriptions = subscriptions;
    this.chatLog = chatLog;
    this.userClient = userClient;
  }

  public async Task PostMessage(string connectionId, string text)
  {
    var subscription = await this.subscriptions.GetSubscription(connectionId);
    var user = await this.userClient.GetUser(subscription.UserId);
    var channel = await this.channelClient.GetChannel(subscription.ChannelId);
    if (!user.MayWrite(channel))
    {
      throw new AuthException("No write access");
    }

    var message = new Message(text, DateTime.Now, user.UserId);
    await this.chatLog.PostMessage(channel, message);

    await this.subscriptions.Broadcast(subscription, message);
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

  public async Task<IEnumerable<Message>> GetBackLog(string connectionId, long offset)
  {
    var subscription = await this.subscriptions.GetSubscription(connectionId);
    return await this.chatLog.GetMessageLogs(await this.channelClient.GetChannel(subscription.ChannelId), offset).ToListAsync();
  }

  public async Task Disconnect(string connectionId)
  {
    await this.subscriptions.Unsubscribe(connectionId);
  }
}
