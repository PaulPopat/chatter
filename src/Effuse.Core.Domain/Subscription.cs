namespace Effuse.Core.Domain;

public class Subscription
{
  public Subscription(string subscriptionId, Guid userId, Guid channelId)
  {
    SubscriptionId = subscriptionId;
    UserId = userId;
    ChannelId = channelId;
  }

  public string SubscriptionId { get; }

  public Guid UserId { get; }

  public Guid ChannelId { get; }
}
