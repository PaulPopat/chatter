using AppPushSubscription = Effuse.Core.Domain.PushSubscription;
using AppUser = Effuse.SSO.Domain.User;

namespace Effuse.SSO.Integration.Clients.PushSubscription;

public interface IPushSubscriptionClient
{
  Task AddSubscription(AppUser user, AppPushSubscription subscription);

  Task<IEnumerable<AppPushSubscription>> GetPushSubscriptions(AppUser user);
}
