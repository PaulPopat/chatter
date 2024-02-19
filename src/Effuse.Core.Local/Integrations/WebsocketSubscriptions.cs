using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Local.Integrations;

public class WebsocketSubscriptions : ISubscriptions
{
  private struct SubsDto
  {
    public List<string> ConnectionIds { get; set; }
  }

  private static string TableName => "Subscriptions";

  private readonly IDatabase database;

  public WebsocketSubscriptions(IDatabase database)
  {
    this.database = database;
  }

  public async Task Broadcast(Channel channel, Message message)
  {
    var subscriptions = await this.database.FindItem<SubsDto>(TableName, channel.ChannelId.ToString());

    if (subscriptions.HasValue) return;

    foreach (var subscription in subscriptions.Value.ConnectionIds ?? new List<string>())
    {
      if (subscription == null) continue;

      WebSocketHandler.Connections[subscription]?.Send(message);
    }
  }

  public async Task Subscribe(Channel channel, string connectionId)
  {
    var subscriptions = await this.database.FindItem<SubsDto>(TableName, channel.ChannelId.ToString());

    if (!subscriptions.HasValue)
    {
      await this.database.AddItem(TableName, channel.ChannelId.ToString(), new SubsDto
      {
        ConnectionIds = new List<string>() { connectionId }
      });
    }
    else
    {
      await this.database.UpdateItem(TableName, channel.ChannelId.ToString(), new SubsDto
      {
        ConnectionIds = subscriptions.Value.ConnectionIds.Append(connectionId).ToList()
      });
    }
  }
}
