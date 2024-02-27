using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Get, "/api/v1/heartbeat")]
public class HeartBeat : IHandler
{
  public Task<HandlerResponse> Handle(HandlerProps props)
  {
    return Task.Run(() => new HandlerResponse(200, new TestHandlerResponse
    {
      Text = "Hello world"
    }));
  }
}
