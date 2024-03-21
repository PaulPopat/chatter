using Effuse.Core.Domain;
using Effuse.Core.Integration;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;
using Effuse.SSO.Domain;
using Effuse.SSO.Integration.Clients.PushSubscription;

namespace Effuse.SSO.Integration;

public class PushSubscriptionClient(IDatabase database) : IPushSubscriptionClient
{
  private struct Dto
  {
    public List<PushSubscriptionDto> Items { get; set; }
  }

  private static string TableName => "PushSubscriptions";

  private static PushSubscriptionDto ToDto(PushSubscription subscription) => new()
  {
    Endpoint = subscription.Endpoint,
    Expires = subscription.Expires.ToISOString(),
    Keys = subscription.Keys.ToDictionary()
  };

  private static PushSubscription FromDto(PushSubscriptionDto dto) => new(dto.Endpoint, DateTime.Parse(dto.Expires), dto.Keys);

  public async Task AddSubscription(User user, PushSubscription subscription)
  {
    var userId = user.UserId.ToString();
    if (await database.Exists(TableName, userId))
    {
      var existing = await database.GetItem<Dto>(TableName, userId);

      var now = DateTime.Now;
      await database.UpdateItem(TableName, userId, new Dto
      {
        Items = [
          .. existing.Items.Select(FromDto).Where(s => s.Expires < now).Select(ToDto),
          ToDto(subscription)]
      });
    }
    else
    {
      await database.AddItem(TableName, userId, new Dto
      {
        Items = [ToDto(subscription)]
      });
    }
  }

  public async Task<IEnumerable<PushSubscription>> GetPushSubscriptions(User user)
  {
    var userId = user.UserId.ToString();
    if (!await database.Exists(TableName, userId))
    {
      return [];
    }

    var now = DateTime.Now;
    var data = await database.GetItem<Dto>(TableName, userId);
    return data.Items.Select(FromDto).Where(s => s.Expires < now);
  }
}
