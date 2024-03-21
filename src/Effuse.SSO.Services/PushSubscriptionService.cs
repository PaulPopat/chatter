using Effuse.Core.Domain;
using Effuse.SSO.Integration.Clients.PushSubscription;
using Effuse.SSO.Integration.Clients.User;

namespace Effuse.SSO.Services;

public class PushSubscriptionService(
  IPushSubscriptionClient pushSubscriptionClient,
  IUserClient userClient,
  AuthService authService)
{
  public async Task Subscribe(string token, string endpoint, DateTime expires, IDictionary<string, string> keys)
  {
    var userId = await authService.Verify(token, UserAccess.Admin);
    var user = await userClient.GetUser(userId);

    await pushSubscriptionClient.AddSubscription(user, new PushSubscription(endpoint, expires, keys));
  }

  public async Task<IEnumerable<PushSubscription>> GetSubscriptions(string token)
  {
    var userId = await authService.Verify(token, UserAccess.Admin);
    var user = await userClient.GetUser(userId);

    return await pushSubscriptionClient.GetPushSubscriptions(user);
  }
}
