using Effuse.Core.Domain;
using Effuse.Server.Domain;

namespace Effuse.Server.Integrations;

public interface IPushSubscriptionClient
{
  Task AddSubscription(User user, PushSubscription subscription);

  Task<IEnumerable<PushSubscription>> GetPushSubscriptions(User user);
}
