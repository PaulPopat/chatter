using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers;

[Route(Method.Get, "/api/v1/user/push-subscriptions")]
public class GetPushSubscriptions(PushSubscriptionService service) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var subscriptions = await service.GetSubscriptions(props.AuthToken);

    return new(200, subscriptions.Select(s => new
    {
      Endpoint = s.Endpoint,
      Expires = s.Expires.ToISOString(),
      Keys = s.Keys
    }));
  }
}
