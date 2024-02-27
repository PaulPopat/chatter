using Effuse.Core.Domain;
using Effuse.Core.Integration;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;

namespace Effuse.Core.Local.Integrations;

public class WebsocketSubscriptions : ISubscriptions
{
  private struct ChannelSubscriptionsDto
  {
    public List<string> ConnectionIds { get; set; }
  }

  private struct SubscriptionDto
  {
    public string ChannelId { get; set; }

    public string UserId { get; set; }
  }

  private struct MessageDto
  {
    public string Type { get; set; }

    public string Text { get; set; }

    public string When { get; set; }

    public string Who { get; set; }
  }

  private static string TableName => "Subscriptions";

  private readonly IDatabase database;

  public WebsocketSubscriptions(IDatabase database)
  {
    this.database = database;
  }

  public async Task Broadcast(Subscription subscription, Message message)
  {
    var subscriptions = await this.database.FindItem<ChannelSubscriptionsDto>(TableName, subscription.ChannelId.ToString());

    if (!subscriptions.HasValue) return;

    MessageDto dto = new()
    {
      Type = "Message",
      Text = message.Text,
      When = message.When.ToISOString(),
      Who = message.UserId.ToString()
    };

    foreach (var connectionId in subscriptions.Value.ConnectionIds ?? new List<string>())
    {
      if (connectionId == null || connectionId == subscription.SubscriptionId) continue;

      WebSocketHandler.Connections[connectionId]?.Send(dto);
    }
  }

  public async Task Subscribe(Subscription subscription)
  {
    var subscriptions = await this.database.FindItem<ChannelSubscriptionsDto>(TableName, subscription.ChannelId.ToString());

    if (!subscriptions.HasValue)
    {
      try
      {

        await this.database.AddItem(TableName, subscription.ChannelId.ToString(), new ChannelSubscriptionsDto
        {
          ConnectionIds = [subscription.SubscriptionId]
        });
      }
      catch (ConflictException)
      {
        await this.Subscribe(subscription);
        return;
      }
    }
    else
    {
      await this.database.UpdateItem(TableName, subscription.ChannelId.ToString(), new ChannelSubscriptionsDto
      {
        ConnectionIds = subscriptions.Value.ConnectionIds.Append(subscription.SubscriptionId).ToList()
      });
    }

    await this.database.AddItem(TableName, subscription.SubscriptionId, new SubscriptionDto
    {
      ChannelId = subscription.ChannelId.ToString(),
      UserId = subscription.UserId.ToString()
    });
  }

  public async Task<Subscription> GetSubscription(string subscriptionId)
  {
    var dto = await this.database.GetItem<SubscriptionDto>(TableName, subscriptionId);
    return new Subscription(
      subscriptionId: subscriptionId,
      userId: Guid.Parse(dto.UserId),
      channelId: Guid.Parse(dto.ChannelId));
  }

  public async Task Unsubscribe(string subscriptionId)
  {
    var subscription = await this.GetSubscription(subscriptionId);
    var subscriptions = await this.database.GetItem<ChannelSubscriptionsDto>(TableName, subscription.ChannelId.ToString());

    await this.database.UpdateItem(TableName, subscription.ChannelId.ToString(), new ChannelSubscriptionsDto
    {
      ConnectionIds = subscriptions.ConnectionIds.Where(c => c != subscriptionId).ToList()
    });

    await this.database.DeleteItem(TableName, subscriptionId);
  }
}
