using Effuse.Core.Domain;
using Effuse.Server.Integrations;

namespace Effuse.Server.Services;

public class PushSubscriptionService(
  IPushSubscriptionClient pushSubscriptionClient,
  AuthService authService)
{
  public async Task Subscribe(string token, string endpoint, DateTime expires, IDictionary<string, string> keys)
  {
    var user = await authService.GetUser(token);

    await pushSubscriptionClient.AddSubscription(user, new PushSubscription(endpoint, expires, keys));
  }
}
