using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models;

namespace Effuse.SSO.Handlers.Controllers;


public class HeartBeat : IHandler<object, TestHandlerResponse>
{
  public Task<HandlerResponse<TestHandlerResponse>> Handle(HandlerProps<object> props)
  {
    return Task.Run(() => new HandlerResponse<TestHandlerResponse>(200, new TestHandlerResponse
    {
      Text = "Hello world"
    }));
  }
}
