using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers;

[Route(Method.Post, "/api/v1/user/push-subscriptions")]
public class SubscribeToPush(PushSubscriptionService service) : IHandler
{
  private struct Form
  {
    public string Endpoint { get; set; }

    public long Expires { get; set; }

    public Dictionary<string, string> Keys { get; set; }
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var body = props.Body<Form>();

    await service.Subscribe(
      props.AuthToken,
      body.Endpoint,
      DateTime.UnixEpoch.AddSeconds(body.Expires),
      body.Keys);

    return new(200, new { Message = "Success" });
  }
}
